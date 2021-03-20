using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

    public bool IsAbilityActive { get; private set; }

    [Header("Input")]

    [SerializeField] private string grabTriggerName;
    [SerializeField] private string dropButtonName;
    private OVRInput.Controller controller;

    [Header("Grab")]

    [SerializeField] private float grabDistance;
    public float GrabDistance { get => grabDistance; }

    [SerializeField] private GameObject potentialGrabbableLaser;

    private bool hasJustGrabbedObject;
    private bool hasJustReleasedObject; // "released" can refer to the object being dropped or shot
    public bool IsGrabbing { get; private set; }

    private Grabbable potentialGrabbable;
    private Grabbable grabbedObject;

    [Header("Shoot")]

    [SerializeField] private GameObject shootAimLaser;
    [SerializeField] private float shootAimLaserLength;
    [SerializeField] private float shootForce;

    // Start is called before the first frame update
    void Start() {
        IsAbilityActive = true;
        controller = GetComponent<TouchController>().Controller;
        shootAimLaser.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis(grabTriggerName) < 0.5) {
            if (hasJustGrabbedObject) {
                hasJustGrabbedObject = false;
            }
            // force player to release 50% of the trigger before being allowed to grab another object
            if (hasJustReleasedObject) {
                hasJustReleasedObject = false;
                if (IsAbilityActive) {
                    potentialGrabbableLaser.SetActive(true);
                }
            }
        }

        if (!IsAbilityActive) {
            return;
        }

        if (!IsGrabbing && !hasJustReleasedObject) {
            CheckForGrabbable();
            if (potentialGrabbable != null && Input.GetAxis(grabTriggerName) == 1) {
                HandleGrab();
            }
        }

        if (IsGrabbing) {
            potentialGrabbableLaser.SetActive(false);
            UpdateShootAimLaser();
            if (Input.GetButtonDown(dropButtonName)) {
                HandleDrop();
            } else if (!hasJustGrabbedObject && Input.GetAxis(grabTriggerName) == 1) {
                HandleShoot();
            }
        }
    }

    public void ActivateGrabAbility() {
        IsAbilityActive = true;
        if (IsGrabbing) {
            shootAimLaser.SetActive(true);
            grabbedObject.gameObject.SetActive(true);
        } else {
            potentialGrabbableLaser.SetActive(true);
        }
    }

    public void DeactivateGrabAbility() {
        IsAbilityActive = false;
        if (IsGrabbing) {
            shootAimLaser.SetActive(false);
            grabbedObject.gameObject.SetActive(false);
        } else {
            potentialGrabbableLaser.SetActive(false);
            if (potentialGrabbable != null) {
                // deactivate outline of previous potnetial grabbable object
                potentialGrabbable.IsOutlineActive = false;
                potentialGrabbable = null;
            }
        }
    }

    /// <summary>
    /// Checks if a Grabbable object is within reach and updates the grab laser.
    /// </summary>
    private void CheckForGrabbable() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        bool isObjectDetected = Physics.Raycast(ray, out hit);
        if (isObjectDetected && hit.transform.CompareTag("Grabbable") && hit.distance <= grabDistance) {
            // potential grabbable object within reach detected
            Grabbable newPotentialGrabbable = hit.transform.GetComponent<Grabbable>();
            if (newPotentialGrabbable == potentialGrabbable) {
                // potential grabbable object previously detected already
                if (!potentialGrabbable.IsOutlineActive) {
                    potentialGrabbable.IsOutlineActive = true;
                }

                UpdatePotentialGrabbableLaser(hit.point);
                return;
            }

            // potential grabbable object is new

            if (potentialGrabbable != null) {
                // dactivate outline of previous potential grabbable object
                potentialGrabbable.IsOutlineActive = false;
            }

            newPotentialGrabbable.IsOutlineActive = true;
            potentialGrabbable = newPotentialGrabbable;
        } else {
            // no potential grabbable object within reach detected
            if (potentialGrabbable != null) {
                // deactivate outline of previous potnetial grabbable object
                potentialGrabbable.IsOutlineActive = false;
                potentialGrabbable = null;
            }
        }

        UpdatePotentialGrabbableLaser(isObjectDetected && hit.distance <= grabDistance
            ? hit.point
            : transform.position + transform.forward * grabDistance);
    }

    /// <summary>
    /// Updates the Transform of the grab laser.
    /// </summary>
    /// <param name="endPoint">The Vector3 end point of the laser.</param>
    private void UpdatePotentialGrabbableLaser(Vector3 endPoint) {
        Vector3 direction = endPoint - transform.position;
        potentialGrabbableLaser.transform.position = transform.position + (direction / 2.0f);
        potentialGrabbableLaser.transform.up = direction;
        potentialGrabbableLaser.transform.localScale = new Vector3(
            potentialGrabbableLaser.transform.localScale.x,
            direction.magnitude / 2.0f,
            potentialGrabbableLaser.transform.localScale.z);
    }

    /// <summary>
    /// Handles grab input.
    /// Assumes there is a potential grabbable object within reach.
    /// </summary>
    private void HandleGrab() {
        IsGrabbing = true;
        hasJustGrabbedObject = true;

        grabbedObject = potentialGrabbable;

        potentialGrabbable.IsOutlineActive = false;
        potentialGrabbable = null;

        // set position and rotation of grabbed object
        grabbedObject.transform.rotation = transform.rotation;
        grabbedObject.transform.position = transform.position;
        grabbedObject.transform.parent = transform;

        grabbedObject.Grab();
        shootAimLaser.SetActive(true);
    }

    /// <summary>
    /// Updates the Transform of the shoot aim laser.
    /// </summary>
    private void UpdateShootAimLaser() {
        Vector3 direction = transform.forward * shootAimLaserLength;
        shootAimLaser.transform.position = grabbedObject.transform.position + (direction / 2.0f);
        shootAimLaser.transform.up = direction;
        shootAimLaser.transform.localScale = new Vector3(
            shootAimLaser.transform.localScale.x,
            direction.magnitude / 2.0f,
            shootAimLaser.transform.localScale.z);
    }

    /// <summary>
    /// Handles drop input.
    /// </summary>
    private void HandleDrop() {
        IsGrabbing = false;

        if (grabbedObject != null) {
            grabbedObject.transform.parent = null;
            grabbedObject.Drop(
                OVRInput.GetLocalControllerVelocity(controller),
                OVRInput.GetLocalControllerAngularVelocity(controller));
            grabbedObject = null;
            hasJustReleasedObject = true;
            shootAimLaser.SetActive(false);
        }
    }

    /// <summary>
    /// Handles shoot input.
    /// </summary>
    private void HandleShoot() {
        IsGrabbing = false;

        if (grabbedObject != null) {
            grabbedObject.transform.parent = null;
            grabbedObject.Shoot(transform.forward, shootForce);
            grabbedObject = null;
            hasJustReleasedObject = true;
            shootAimLaser.SetActive(false);
        }
    }
}

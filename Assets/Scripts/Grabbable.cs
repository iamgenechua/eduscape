using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(Outline))]
public class Grabbable : MonoBehaviour {

    private MeshRenderer mesh;
    private Rigidbody rb;
    private AudioSource dropAudio;
    private Outline outline;

    // [SerializeField] private float grabbableOffset = 0.2f;
    [Range(0, 1)]
    [SerializeField] private float decelerationModifier = 0.9f;
    public Vector3 ReleasePos { get; private set; } // "release" can refer to shooting or dropping

    private bool isDecelerating = false;

    public bool IsOutlineActive { get => outline.enabled; set => outline.enabled = value; }

    // Start is called before the first frame update
    void Start() {
        mesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        dropAudio = GetComponent<AudioSource>();
        outline = GetComponent<Outline>();
        IsOutlineActive = false;
    }

    // Update is called once per frame
    void Update() {
        if (isDecelerating) {
            Decelerate();
        }
    }

    /// <summary>
    /// Handles this Grabbable getting grabbed.
    /// </summary>
    public void Grab() {
        rb.isKinematic = true;
        SetTransparency(0.6f);
    }

    /// <summary>
    /// Sets the transparency of this object to the target transparency.
    /// </summary>
    /// <param name="targetTransparency">The transparency value to set to, from 0 to 1 inclusive.</param>
    private void SetTransparency(float targetTransparency) {
        if (targetTransparency < 0f || targetTransparency > 1f) {
            Debug.LogError("Target Transparency must be between 0 and 1 inclusive.");
            return;
        }

        Color color = mesh.material.color;
        color.a = targetTransparency;
        mesh.material.color = color;
    }

    /// <summary>
    /// Handles this Grabbable getting dropped.
    /// </summary>
    /// <param name="velocity">The starting Vector3 velocity.</param>
    /// <param name="angularVelocity">The starting Vector3 angular velocity.</param>
    public void Drop(Vector3 velocity, Vector3 angularVelocity) {
        ReleasePos = transform.position;

        rb.isKinematic = false;
        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
        SetTransparency(1f);
    }

    /// <summary>
    /// Handles this Grabbable being shot.
    /// </summary>
    /// <param name="direction">The Vector3 direction of the shot.</param>
    /// <param name="force">The force of the shot as a float.</param>
    public void Shoot(Vector3 direction, float force) {
        ReleasePos = transform.position;

        rb.isKinematic = false;
        rb.velocity = direction * force;
        SetTransparency(1f);
    }

    /// <summary>
    /// Reduces the speed of the object.
    /// </summary>
    private void Decelerate() {
        if (rb.velocity.sqrMagnitude < 0.001f) {
            rb.velocity = Vector3.zero;
            ReleasePos = transform.position;
            isDecelerating = false;
            return;
        }

        rb.velocity *= decelerationModifier;
    }

    private void OnCollisionEnter(Collision collision) {
        if (rb == null || dropAudio == null) {
            return;
        }

        if (collision.gameObject.CompareTag("Ground")) {
            // hit the ground, play drop audio
            dropAudio.Play();
        } else {
            // hit some other object, reduce speed
            rb.velocity *= 0.6f;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground") && !isDecelerating && rb.velocity.sqrMagnitude > 0) {
            // hit the ground, begin deceleration
            isDecelerating = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            // left the ground, stop deceleration
            isDecelerating = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    private Rigidbody rb;

    public bool HasLaunched { get; private set; }

    [Header("Cockpit Door")]

    [SerializeField] private Door cockpitDoor;

    [Header("Ramp")]

    [SerializeField] private BoxCollider rampClosingCollider;

    [SerializeField] private Animator rampAnim;
    [SerializeField] private string rampAnimParam = "isOpen";

    [Header("Summary and Button Stations")]

    [SerializeField] private FadeCanvas[] screenFadeCanvases;
    [SerializeField] private Animator[] buttonCoverAnims;
    [SerializeField] private string buttonCoverAnimParam = "isOpen";

    [Header("Flight")]

    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxForwardSpeed;
    private float currSpeed = 0f;

    [SerializeField] private ShipTarget shipTarget;

    private bool isRising = false;
    private bool isChasingTarget = false;

    private float distanceMaintainedFromTarget = 0f;

    /// <summary>
    /// Gets the vector representing the forward direction of the ship.
    /// </summary>
    /// <remarks>
    /// The components of the ship have been rotated in so many different ways that
    /// <c>transform.forward</c> by itself doesn't give the direction the ship looks like it is facing.
    /// This method converts <c>transform.forward</c> into the direction the ship looks like it's facing.
    /// </remarks>
    /// <returns>The converted Vector3.</returns>
    public Vector3 TransformForward { get => -transform.forward; }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();

        HasLaunched = false;
        cockpitDoor.OpenDoor();
        rampClosingCollider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (isChasingTarget) {
            ChaseTarget();
        }
    }

    public void Launch() {
        // blast off into space
        HasLaunched = true;
        CloseRamp();

        LevelManager.Instance.Player.transform.parent = transform;
        StartCoroutine(RunFlightPath());

        UnlockSummaryAndButtons();
    }

    public void CloseRamp() {
        rampAnim.SetBool(rampAnimParam, false);

        // prevent the player from teleporting out of the ship while the ramp is closing
        rampClosingCollider.gameObject.SetActive(true);
        StartCoroutine(GameManager.Instance.WaitForConditionBeforeAction(
            () => rampAnim.GetCurrentAnimatorStateInfo(0).IsName("Ramp Closed"),
            () => rampClosingCollider.gameObject.SetActive(false)));
    }

    public void UnlockSummaryAndButtons() {
        foreach (FadeCanvas fadeCanvas in screenFadeCanvases) {
            fadeCanvas.FadeIn();
        }

        foreach (Animator anim in buttonCoverAnims) {
            anim.SetBool(buttonCoverAnimParam, true);
        }
    }

    private IEnumerator RunFlightPath() {
        StartCoroutine(Rise(riseHeight));
        
        yield return new WaitUntil(() => !isRising);
        
        shipTarget.transform.position = new Vector3(
            shipTarget.transform.position.x,
            transform.position.y,
            shipTarget.transform.position.z);

        distanceMaintainedFromTarget = Vector3.Distance(transform.position, shipTarget.transform.position);
        shipTarget.StartMoving();
        isChasingTarget = true;
    }

    private IEnumerator Rise(float targetHeight) {
        isRising = true;

        float startHeight = transform.position.y;
        while (transform.position.y - startHeight < targetHeight) {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            yield return null;
        }

        isRising = false;
    }

    private void ChaseTarget() {
        // look at target
        Vector3 relativePos = transform.position - shipTarget.transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        if (Vector3.Distance(transform.position, shipTarget.transform.position) > distanceMaintainedFromTarget) {
            // accelerate
            currSpeed += acceleration * Time.deltaTime;
        } else {
            // decelerate
            currSpeed -= acceleration * Time.deltaTime;
        }

        currSpeed = Mathf.Clamp(currSpeed, 0f, maxForwardSpeed);

        transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, currSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    private ShipRamp ramp;

    public bool HasLaunched { get; private set; }

    [Header("Engines")]

    [SerializeField] private ShipEngine mainEngine;
    [SerializeField] private ShipEngine[] engines;

    [Header("Cockpit Door")]

    [SerializeField] private Door cockpitDoor;

    [Header("Summary and Button Stations")]

    [SerializeField] private FadeText summaryFadeCanvas;
    [SerializeField] private DisplayScreen[] gameOptionsScreens;
    [SerializeField] private string[] gameOptionsText;
    [SerializeField] private PressableButtonCover[] gameOptionButtonCovers;

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

    void Awake() {
        ramp = GetComponentInChildren<ShipRamp>();
    }

    // Start is called before the first frame update
    void Start() {
        HasLaunched = false;
    }

    // Update is called once per frame
    void Update() {
        if (isChasingTarget) {
            ChaseTarget();
        }
    }

    public void Launch() {
        if (!System.Array.TrueForAll(engines, engine => engine.IsHeated)) {
            // instruct player to heat engines
        }

        // blast off into space
        HasLaunched = true;
        mainEngine.Heat();
        ramp.CloseRamp();

        LevelManager.Instance.Player.transform.parent = transform;
        StartCoroutine(RunFlightPath());

        UnlockSummaryAndButtons();
    }

    public void UnlockSummaryAndButtons() {
        foreach (PressableButtonCover cover in gameOptionButtonCovers) {
            cover.Open();
        }

        summaryFadeCanvas.FadeIn();

        if (gameOptionsScreens.Length != gameOptionsText.Length) {
            Debug.LogError("Game Options Screens length not equal to Game Options Text length.");
            return;
        }

        for (int i = 0; i < gameOptionsScreens.Length; i++) {
            gameOptionsScreens[i].SetText(gameOptionsText[i]);
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

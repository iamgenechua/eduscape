using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipController : MonoBehaviour {

    private ShipRamp ramp;

    public bool HasLaunched { get; private set; }

    [Header("Main Engine")]

    [SerializeField] private ShipEngine mainEngine;
    [SerializeField] private AudioSource mainEngineAudioSource;
    [SerializeField] private AudioClip defaultHeatedSound;
    [SerializeField] private AudioClip intenseHeatedSound;

    [Header("Engines")]

    [SerializeField] private ShipEngine[] engines;

    [Header("Cockpit Door")]

    [SerializeField] private Door cockpitDoor;

    [Header("Summary and Button Stations")]

    [SerializeField] private FadeText summaryFadeCanvas;
    [SerializeField] private DisplayScreen[] gameOptionsScreens;
    [SerializeField] private string[] gameOptionsText;
    [SerializeField] private PressableButtonCover[] gameOptionButtonCovers;

    [Header("Launch")]

    [SerializeField] private AudioSource launchAudioSource;
    [SerializeField] private AudioClip startupBuild;
    [SerializeField] private AudioClip startupLoop;
    [SerializeField] private AudioClip startupFail;
    [SerializeField] private AudioClip startupSuccess;

    [Space(10)]

    [SerializeField] private UnityEvent launchEvent;

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

    [SerializeField] private ActionBlocker[] flightActionBlockers;

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

        foreach (DisplayScreen screen in gameOptionsScreens) {
            screen.DeactivateScreen();
        }

        foreach (ActionBlocker actionBlocker in flightActionBlockers) {
            actionBlocker.Deactivate();
        }
    }

    // Update is called once per frame
    void Update() {
        if (isChasingTarget) {
            ChaseTarget();
        }
    }

    public void PrimeLaunch() {
        if (!HasLaunched) {
            StartCoroutine(AttemptLaunch());
        }
    }

    private IEnumerator AttemptLaunch() {
        bool willAttemptSucceed = System.Array.TrueForAll(engines, engine => engine.IsHeated);

        launchAudioSource.clip = startupBuild;
        launchAudioSource.Play();

        yield return new WaitForSeconds(startupBuild.length);

        foreach (ShipEngine engine in engines) {
            if (!engine.IsHeated) {
                engine.Heat();
            }
        }

        launchAudioSource.clip = startupLoop;
        launchAudioSource.loop = true;
        launchAudioSource.Play();

        yield return new WaitForSeconds(3f);

        launchAudioSource.loop = false;

        if (willAttemptSucceed) {
            launchAudioSource.clip = startupSuccess;
            launchAudioSource.Play();
            Launch();
            yield break;
        }

        foreach (ShipEngine engine in engines) {
            engine.Cool();
        }

        launchAudioSource.clip = startupFail;
        launchAudioSource.Play();
    }

    private void Launch() {
        HasLaunched = true;
        mainEngine.Heat();
        ramp.CloseRamp();

        LevelManager.Instance.IsProjectileNetDestroyerEnabled = false;
        foreach (ActionBlocker actionBlocker in flightActionBlockers) {
            actionBlocker.Activate();
        }

        LevelManager.Instance.Player.transform.parent = transform;
        StartCoroutine(RunFlightPath());

        UnlockSummaryAndButtons();

        launchEvent.Invoke();
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
            gameOptionsScreens[i].ActivateScreen();
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

        mainEngineAudioSource.clip = intenseHeatedSound;
        mainEngineAudioSource.Play();

        float startHeight = transform.position.y;
        while (transform.position.y - startHeight < targetHeight) {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            yield return null;
        }

        mainEngineAudioSource.clip = defaultHeatedSound;
        mainEngineAudioSource.Play();

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

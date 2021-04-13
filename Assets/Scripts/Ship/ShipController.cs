using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipController : MonoBehaviour {

    private ShipRamp ramp;
    private ShipCamera cam;

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

    [Header("Main Display")]

    [SerializeField] private DisplayScreen mainDisplay;

    [Header("Summary and Button Stations")]

    [SerializeField] private RationaleSummary summary;
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
    [SerializeField] private UnityEvent launchFailEvent;

    private bool isAttemptingLaunch = false;

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

    void Awake() {
        ramp = GetComponentInChildren<ShipRamp>();
        cam = GetComponentInChildren<ShipCamera>();
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

        mainDisplay.SetText("LAUNCH");
    }

    // Update is called once per frame
    void Update() {
        if (isChasingTarget) {
            ChaseTarget();
        }
    }

    public void PrimeLaunch() {
        if (!HasLaunched && !isAttemptingLaunch) {
            StartCoroutine(AttemptLaunch());
        }
    }

    private IEnumerator AttemptLaunch() {
        isAttemptingLaunch = true;
        bool willAttemptSucceed = System.Array.TrueForAll(engines, engine => engine.IsHeated);

        mainDisplay.SetText("Priming Launch...");
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

        int secondsToWait = 3;
        for (int i = 0; i < secondsToWait; i++) {
            mainDisplay.SetText($"{secondsToWait - i}", false);
            yield return new WaitForSeconds(1f);
        }

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

        mainDisplay.SetText("LAUNCH FAILED", false);
        launchAudioSource.clip = startupFail;
        launchAudioSource.Play();

        yield return new WaitForSeconds(5f);
        mainDisplay.SetText("It's no good. We need to heat the engines from outside the ship!");
        launchFailEvent.Invoke();

        yield return new WaitUntil(() => !mainDisplay.IsRollingOut);
        isAttemptingLaunch = false;
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

        summary.Activate();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private static TutorialManager _instance;
    public static TutorialManager Instance { get => _instance; }

    public enum TutorialStage { WAKEUP, WARNING, TRANSFERRING, CYCLE, SHOOT, COMPLETE }
    public TutorialStage CurrTutorialStage { get; private set; }

    [Header("Bedroom Lights")]

    [SerializeField] private SciFiLight[] bedroomLights;

    [Header("Docking Screens")]

    [SerializeField] private string wakeUpText = "Good day!";
    [SerializeField] private string greetingText = "It's EDU, your station AI ^u^";
    [SerializeField] private string interruptedText = "The time now is";
    [SerializeField] private string warningText = "WARNING";
    [SerializeField] private string stationFailureText = "Station on collision course";
    [SerializeField] private string transferText = "We need to leave!";
    [SerializeField] private string buttonTipText = "Press the button";
    [SerializeField] private DisplayScreen[] dockingScreens;

    [Header("Transfer")]

    [SerializeField] private Animator transferButtonAnim;
    [SerializeField] private string transferButtonOpenParam;

    [SerializeField] private Door podDoor;
    [SerializeField] private SciFiLight corridorLight;
    [SerializeField] private Door stationDoor;

    [Header("Elements")]

    [SerializeField] private PlayerElements playerElements;
    [SerializeField] private DisplayScreen elementScreen;
    [SerializeField] private string wellDoneText = "Well done";
    [SerializeField] private string cycleText = "Switch elements with the right hand grip";
    [SerializeField] private string shootText = "Shoot elements with the right hand trigger";
    [SerializeField] private string completeTutorialText = "Time for you to go now";

    void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StartTutorial());
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void SetDockingScreensTexts(string text) {
        foreach (DisplayScreen screen in dockingScreens) {
            screen.SetText(text);
        }
    }

    public IEnumerator StartTutorial() {
        // reset settings
        transferButtonAnim.SetBool(transferButtonOpenParam, false);
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
        foreach (SciFiLight light in bedroomLights) {
            light.TurnOff();
        }
        // TODO: check for more settings to reset

        CurrTutorialStage = TutorialStage.WAKEUP;

        yield return new WaitForSeconds(5f);

        foreach (SciFiLight light in bedroomLights) {
            light.TurnOn();
        }

        SetDockingScreensTexts(wakeUpText);

        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(greetingText);

        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(interruptedText);

        yield return new WaitForSeconds(3f);
        
        StartCoroutine(Warning());
    }

    private IEnumerator Warning() {
        CurrTutorialStage = TutorialStage.WARNING;

        // turn screen red
        SetDockingScreensTexts(warningText);
        foreach (SciFiLight light in bedroomLights) {
            light.TurnOnDanger();
        }

        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(stationFailureText);

        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(transferText);
        transferButtonAnim.SetBool(transferButtonOpenParam, true);

        yield return new WaitForSeconds(5f);

        if (CurrTutorialStage == TutorialStage.WARNING) {
            SetDockingScreensTexts(buttonTipText);
        }
    }

    public void BeginTransfer() {
        if (CurrTutorialStage == TutorialStage.WARNING) {
            CurrTutorialStage = TutorialStage.TRANSFERRING;
            podDoor.OpenDoor();
        }
    }

    public void CompleteTeleportTutorial() {
        corridorLight.TurnOn();
        stationDoor.OpenDoor();
    }

    public void TeachElementCycling() {
        CurrTutorialStage = TutorialStage.CYCLE;
        podDoor.CloseDoor();
        stationDoor.CloseDoor();

        elementScreen.SetText(cycleText);
        elementScreen.Unstow();
        playerElements.SwitchToElementEvent.AddListener(CompleteElementCycling);
    }

    private void CompleteElementCycling() {
        StartCoroutine(TeachElementShooting());
    }

    private IEnumerator TeachElementShooting() {
        playerElements.SwitchToElementEvent.RemoveListener(CompleteElementCycling);
        elementScreen.SetText(wellDoneText);

        yield return new WaitForSeconds(3f);
        
        CurrTutorialStage = TutorialStage.SHOOT;
        elementScreen.SetText(shootText);
        playerElements.ShootElementEvent.AddListener(CompleteTutorial);
    }

    private void CompleteTutorial() {
        StartCoroutine(DisplayFinalInstructions());
    }

    private IEnumerator DisplayFinalInstructions() {
        playerElements.ShootElementEvent.RemoveListener(CompleteTutorial);
        elementScreen.SetText(wellDoneText);

        yield return new WaitForSeconds(3f);

        CurrTutorialStage = TutorialStage.COMPLETE;
        elementScreen.SetText(completeTutorialText);

        yield return new WaitForSeconds(5f);

        elementScreen.Stow();
    }
}

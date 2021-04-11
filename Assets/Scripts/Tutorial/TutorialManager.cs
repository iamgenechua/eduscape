using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private static TutorialManager _instance;
    public static TutorialManager Instance { get => _instance; }

    public enum TutorialStage { WAKEUP, WARNING, TRANSFERRING, CYCLE, SHOOT, DOOR_PANEL, COMPLETE }
    public TutorialStage CurrTutorialStage { get; private set; }

    [Header("Bedroom Lights")]

    [SerializeField] private SciFiLight[] bedroomLights;

    [Header("Bedroom Screens")]

    [SerializeField] private string wakeUpText = "Good day!";
    [SerializeField] private string greetingText = "It's EDU, your station AI ^u^";
    [SerializeField] private string interruptedText = "The time now is";
    [SerializeField] private string warningText = "WARNING";
    [SerializeField] private string stationFailureText = "Station on collision course";
    [SerializeField] private string transferText = "We need to leave!";
    [SerializeField] private string buttonTipText = "Press the button";
    [SerializeField] private DisplayScreen[] bedroomScreens;

    [Header("Transfer")]

    [SerializeField] private AudioSource alarmAudioSource;

    [SerializeField] private PressableButtonCover transferButtonCover;

    [SerializeField] private Door podDoor;
    [SerializeField] private SciFiLight corridorLight;
    [SerializeField] private Door stationDoor;

    [Header("Elements")]

    [SerializeField] private PlayerElements playerElements;
    [SerializeField] private DisplayScreen elementScreen;
    [SerializeField] private string cycleText = "Summon matter with the right hand grip";
    [SerializeField] private string shootText = "Shoot with the right hand trigger";
    [SerializeField] private string elementsCompleteText = "Well done";

    [Header("Door Panel")]

    [SerializeField] private DoorPanel startDoorPanel;
    [SerializeField] private DisplayScreen doorPanelInstructionScreen;
    [SerializeField] private string doorPanelExplanationText = "The door panel is busted";
    [SerializeField] private string doorPanelInstructionText = "You'll have to shoot it to get out";

    [SerializeField] private Door startDoor;
    [SerializeField] private string completeTutorialText = "Excellent\nTime to go!";

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

    }

    // Update is called once per frame
    void Update() {

    }

    private void SetDockingScreensTexts(string text, bool rollOut = true) {
        foreach (DisplayScreen screen in bedroomScreens) {
            screen.SetText(text, rollOut);
        }
    }

    private void ResetTutorial() {
        CurrTutorialStage = TutorialStage.WAKEUP;

        transferButtonCover.Close();
        podDoor.CloseDoor();
        
        corridorLight.TurnOff();
        stationDoor.CloseDoor();
        
        elementScreen.Stow();

        startDoor.CloseDoor();
        startDoorPanel.SwitchOff();
        doorPanelInstructionScreen.DeactivateScreen();

        foreach (DisplayScreen screen in bedroomScreens) {
            screen.DeactivateScreen();
        }

        foreach (SciFiLight light in bedroomLights) {
            light.TurnOff();
        }
    }

    public void StartTutorial() {
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial() {
        ResetTutorial();

        MusicManager.Instance.PlayIntroMusic(5f);

        yield return new WaitForSeconds(MusicManager.Instance.UseSongForIntro ? 4f : 4.4f);

        foreach (SciFiLight light in bedroomLights) {
            light.TurnOn();
        }

        yield return new WaitForSeconds(MusicManager.Instance.UseSongForIntro ? 3f : 3.6f);
        
        SetDockingScreensTexts(wakeUpText);
        foreach (DisplayScreen screen in bedroomScreens) {
            screen.ActivateScreen();
        }

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(greetingText);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(interruptedText);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(MusicManager.Instance.UseSongForIntro ? 6f : 4f);

        MusicManager.Instance.StopIntroMusic();

        StartCoroutine(Warning());
    }

    private IEnumerator Warning() {
        CurrTutorialStage = TutorialStage.WARNING;

        SetDockingScreensTexts(warningText, false);
        foreach (DisplayScreen screen in bedroomScreens) {
            screen.DisplayWarning();
        }

        foreach (SciFiLight light in bedroomLights) {
            light.TurnOnDanger();
        }

        alarmAudioSource.Play();

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsPulsingWarningScreen));

        SetDockingScreensTexts(stationFailureText, false);

        yield return new WaitForSeconds(5f);

        SetDockingScreensTexts(transferText);
        transferButtonCover.Open();

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(10f);
        
        if (CurrTutorialStage == TutorialStage.WARNING) {
            SetDockingScreensTexts(buttonTipText);
        }
    }

    public void BeginTransfer() {
        if (CurrTutorialStage == TutorialStage.WARNING) {
            CurrTutorialStage = TutorialStage.TRANSFERRING;
            alarmAudioSource.loop = false;
            podDoor.OpenDoor();
        }
    }

    public void CompleteTeleportTutorial() {
        corridorLight.TurnOn();
        stationDoor.OpenDoor();
    }

    public IEnumerator TeachElementCycling() {
        CurrTutorialStage = TutorialStage.CYCLE;
        elementScreen.Unstow();

        yield return new WaitUntil(() => !elementScreen.IsStowed);
        yield return new WaitForSeconds(0.5f);

        elementScreen.SetText(cycleText);
        playerElements.SwitchToElementEvent.AddListener(CompleteElementCycling);
    }

    private void CompleteElementCycling() {
        StartCoroutine(TeachElementShooting());
    }

    private IEnumerator TeachElementShooting() {
        playerElements.SwitchToElementEvent.RemoveListener(CompleteElementCycling);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        
        CurrTutorialStage = TutorialStage.SHOOT;
        elementScreen.SetText(shootText);
        playerElements.ShootElementEvent.AddListener(CompleteElementsTutorial);
    }

    private void CompleteElementsTutorial() {
        StartCoroutine(DisplayDoorPanelInstructions());
    }

    private IEnumerator DisplayDoorPanelInstructions() {
        playerElements.ShootElementEvent.RemoveListener(CompleteElementsTutorial);
        elementScreen.SetText(elementsCompleteText);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(3f);

        elementScreen.Stow();

        CurrTutorialStage = TutorialStage.DOOR_PANEL;
        doorPanelInstructionScreen.ActivateScreen();
        doorPanelInstructionScreen.SetText(doorPanelExplanationText);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));
        yield return new WaitForSeconds(8f);

        doorPanelInstructionScreen.SetText(doorPanelInstructionText);
        startDoorPanel.SwitchOn();
        startDoor.OpenEvent.AddListener(CompleteTutorial);
    }

    public void CompleteTutorial() {
        StartCoroutine(DisplayFinalInstruction());
    }

    private IEnumerator DisplayFinalInstruction() {
        startDoor.OpenEvent.RemoveListener(CompleteTutorial);

        yield return new WaitUntil(() => System.Array.TrueForAll(bedroomScreens, screen => !screen.IsRollingOut));

        CurrTutorialStage = TutorialStage.COMPLETE;
        doorPanelInstructionScreen.SetText(completeTutorialText);
    }
}

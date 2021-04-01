using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour {

    private static TutorialManager _instance;
    public static TutorialManager Instance { get => _instance; }

    public enum TutorialStage { DOCKING, DOCKED, TRANSFERRING, CYCLE, SHOOT, COMPLETE }
    public TutorialStage CurrTutorialStage { get; private set; }

    [Header("Docking Screens")]

    [SerializeField] private string dockingText = "Docking with Eduscape Station...";
    [SerializeField] private string dockedText = "Docking Complete";
    [SerializeField] private string transferText = "Begin Transfer to Station";
    [SerializeField] private DisplayScreen[] dockingScreens;

    [Header("Transfer")]

    [SerializeField] private Animator transferButtonAnim;
    [SerializeField] private string transferButtonOpenParam;

    [SerializeField] private Door podDoor;
    [SerializeField] private MeshRenderer corridorLightMesh;
    [SerializeField] private Material corridorLightLitMaterial;
    [SerializeField] private Light corridorLight;
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
        StartCoroutine(StartDocking());
    }

    // Update is called once per frame
    void Update() {
        
    }

    public IEnumerator StartDocking() {
        CurrTutorialStage = TutorialStage.DOCKING;

        transferButtonAnim.SetBool(transferButtonOpenParam, false);
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
        foreach (DisplayScreen screen in dockingScreens) {
            screen.SetText(dockingText);
        }

        yield return new WaitForSeconds(5f);
        
        StartCoroutine(Dock());
    }

    private IEnumerator Dock() {
        CurrTutorialStage = TutorialStage.DOCKED;
        foreach (DisplayScreen screen in dockingScreens) {
            screen.SetText(dockedText);
        }

        yield return new WaitForSeconds(5f);

        transferButtonAnim.SetBool(transferButtonOpenParam, true);
        foreach (DisplayScreen screen in dockingScreens) {
            screen.SetText(transferText);
        }
    }

    public void BeginTransfer() {
        if (CurrTutorialStage == TutorialStage.DOCKED) {
            CurrTutorialStage = TutorialStage.TRANSFERRING;
            podDoor.OpenDoor();
        }
    }

    public void CompleteTeleportTutorial() {
        Material[] corridorLightMaterials = corridorLightMesh.materials;
        corridorLightMaterials[1] = corridorLightLitMaterial;
        corridorLightMesh.materials = corridorLightMaterials;

        corridorLight.gameObject.SetActive(true);
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

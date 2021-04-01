using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour {

    public enum TutorialStage { DOCKING, DOCKED, TRANSFERRING, CYCLE, SHOOT, COMPLETE }
    private TutorialStage currStage;

    [Header("Docking Screens")]

    [SerializeField] private string dockingText = "Docking with Eduscape Station...";
    [SerializeField] private string dockedText = "Docking Complete";
    [SerializeField] private string transferText = "Begin Transfer to Station";
    [SerializeField] private DisplayScreen[] dockingScreens;

    [Header("Transfer Button")]

    [SerializeField] private Animator transferButtonAnim;
    [SerializeField] private string transferButtonOpenParam;

    [Header("Doors")]

    [SerializeField] private Door podDoor;
    [SerializeField] private Door stationDoor;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StartDocking());
    }

    // Update is called once per frame
    void Update() {
        
    }

    public IEnumerator StartDocking() {
        currStage = TutorialStage.DOCKING;

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
        currStage = TutorialStage.DOCKED;
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
        if (currStage == TutorialStage.DOCKED) {
            StartCoroutine(Transfer());
        }
    }

    private IEnumerator Transfer() {
        currStage = TutorialStage.TRANSFERRING;
        podDoor.OpenDoor();
        yield return new WaitForSeconds(0.5f);
        stationDoor.OpenDoor();
    }

    private void TeachElementCycling() {
        currStage = TutorialStage.CYCLE;
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
    }

    private void OnTriggerEnter(Collider other) {
        if (currStage == TutorialStage.TRANSFERRING && other.CompareTag("Player")) {
            TeachElementCycling();
        }
    }
}

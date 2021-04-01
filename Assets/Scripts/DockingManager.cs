using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DockingManager : MonoBehaviour {

    public enum DockingStage { DOCKING, DOCKED, TRANSFERRING, COMPLETE }
    private DockingStage currDockingStage;

    [Header("Docking Screens")]

    [SerializeField] private string dockingText = "Docking with Eduscape Station...";
    [SerializeField] private string dockedText = "Docking Complete";
    [SerializeField] private string transferText = "Begin Transfer to Station";
    [SerializeField] private TextMeshProUGUI[] dockingScreensTexts;

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
        currDockingStage = DockingStage.DOCKING;

        transferButtonAnim.SetBool(transferButtonOpenParam, false);
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
        foreach (TextMeshProUGUI text in dockingScreensTexts) {
            text.text = dockingText;
        }

        yield return new WaitForSeconds(5f);
        
        StartCoroutine(Dock());
    }

    private IEnumerator Dock() {
        currDockingStage = DockingStage.DOCKED;
        foreach (TextMeshProUGUI text in dockingScreensTexts) {
            text.text = dockedText;
        }

        yield return new WaitForSeconds(5f);

        transferButtonAnim.SetBool(transferButtonOpenParam, true);
        foreach (TextMeshProUGUI text in dockingScreensTexts) {
            text.text = transferText;
        }
    }

    public void BeginTransfer() {
        if (currDockingStage == DockingStage.DOCKED) {
            StartCoroutine(Transfer());
        }
    }

    private IEnumerator Transfer() {
        currDockingStage = DockingStage.TRANSFERRING;
        podDoor.OpenDoor();
        yield return new WaitForSeconds(0.5f);
        stationDoor.OpenDoor();
    }

    private void CompleteTransfer() {
        currDockingStage = DockingStage.COMPLETE;
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
    }

    private void OnTriggerEnter(Collider other) {
        if (currDockingStage == DockingStage.TRANSFERRING && other.CompareTag("Player")) {
            CompleteTransfer();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DockingManager : MonoBehaviour {

    public enum DockingStage { DOCKING, DOCKED, TRANSFERRING, COMPLETE }
    private DockingStage currDockingStage;

    [SerializeField] private TextMeshProUGUI text;
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
        text.text = "Docking with Eduscape Station...";
        // lock transfer button
        podDoor.CloseDoor();
        stationDoor.CloseDoor();
        yield return new WaitForSeconds(5f);
        StartCoroutine(Dock());
    }

    private IEnumerator Dock() {
        currDockingStage = DockingStage.DOCKED;
        text.text = "Docking Complete";
        yield return new WaitForSeconds(5f);
        text.text = "Begin Transfer to Station";
        StartCoroutine(BeginTransfer()); // Unlock Transfer Button
    }

    private IEnumerator BeginTransfer() {
        currDockingStage = DockingStage.TRANSFERRING;
        podDoor.OpenDoor();
        yield return new WaitForSeconds(1f);
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

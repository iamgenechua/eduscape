using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomThreshold : MonoBehaviour {

    [SerializeField] private Door startAreaDoor;
    [SerializeField] private ElementTarget startAreaDoorPanel;

    [Tooltip("Object containing start area light objects.")]
    [SerializeField] private GameObject startAreaLights;

    private void AdvanceStage() {
        // switch off start area lights, disable the door panel, and shut the door
        startAreaLights.SetActive(false);
        startAreaDoorPanel.enabled = false;
        startAreaDoor.CloseDoor();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            AdvanceStage();
        }
    }
}

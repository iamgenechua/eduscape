using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomThreshold : MonoBehaviour {

    [SerializeField] private Door startAreaDoor;

    private void AdvanceStage() {
        startAreaDoor.CloseDoor();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            AdvanceStage();
        }
    }
}

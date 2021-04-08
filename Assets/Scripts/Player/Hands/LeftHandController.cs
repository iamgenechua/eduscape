using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour {

    [SerializeField] private string teleportTriggerName;
    private bool isTeleportTriggerHeld = false;

    private TeleportRay teleportRay;

    // Start is called before the first frame update
    void Start() {
        teleportRay = GetComponent<TeleportRay>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis(teleportTriggerName) < 1 && Input.GetKeyUp(KeyCode.C)) {
            teleportRay.DeactivateRay();
            if (isTeleportTriggerHeld) {
                isTeleportTriggerHeld = false;
                teleportRay.Teleport();
            }
        }

        if (Input.GetAxis(teleportTriggerName) == 1 || Input.GetKeyDown(KeyCode.C)) {
            isTeleportTriggerHeld = true;
            teleportRay.RenderRay(transform.position, transform.forward);
        }
    }
}

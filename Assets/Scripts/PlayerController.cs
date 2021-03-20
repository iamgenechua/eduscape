using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Grab grab;
    private PlayerElements playerElements;

    [SerializeField] private string cycleTriggerName;
    private bool hasJustCycled = false;

    // Start is called before the first frame update
    void Start() {
        grab = GetComponentInChildren<Grab>();
        playerElements = GetComponentInChildren<PlayerElements>();
    }

    // Update is called once per frame
    void Update() {
        if (hasJustCycled && Input.GetAxis(cycleTriggerName) < 0.9) {
            hasJustCycled = false;
        }

        if (!hasJustCycled && Input.GetAxis(cycleTriggerName) == 1) {
            HandleCycle();
        }
    }

    private void HandleCycle() {
        hasJustCycled = true;
        playerElements.CycleActiveElement();
        if (playerElements.ActiveElement == null) {
            grab.ActivateGrabAbility();
        } else {
            grab.DeactivateGrabAbility();
        }
    }
}

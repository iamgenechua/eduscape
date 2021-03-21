using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Grab grab;
    private PlayerElements playerElements;

    [SerializeField] private string cycleTriggerName;
    private bool hasJustCycled = false;

    [SerializeField] private string shootTriggerName;
    private bool hasJustShotElement = false;

    [SerializeField] private TouchController elementsController;

    // Start is called before the first frame update
    void Start() {
        grab = GetComponentInChildren<Grab>();
        playerElements = GetComponentInChildren<PlayerElements>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            HandleCycle();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            HandleShootElement();
        }
        float cycleInput = Input.GetAxis(cycleTriggerName);

        if (hasJustCycled && cycleInput < 0.9) {
            hasJustCycled = false;
        }

        if (!hasJustCycled && cycleInput == 1) {
            HandleCycle();
        }

        float shootInput = Input.GetAxis(shootTriggerName);

        if (hasJustShotElement && shootInput < 0.5) {
            hasJustShotElement = false;
        }

        if (!hasJustShotElement && !grab.HasJustGrabbedObject && shootInput == 1) {
            HandleShootElement();
        }
    }

    private void HandleCycle() {
        hasJustCycled = true;
        playerElements.CycleActiveElement();
        if (playerElements.ActiveElement == null) {
            grab.ActivateGrabAbility();
            elementsController.OffsetRotation = elementsController.DefaultRotation;
        } else {
            grab.DeactivateGrabAbility();
            elementsController.OffsetRotation = elementsController.HoldElementsRotation;
        }
    }

    private void HandleShootElement() {
        if (playerElements.ActiveElement == null) {
            return;
        }

        hasJustShotElement = true;
        StartCoroutine(playerElements.ShootActiveElement());
    }
}

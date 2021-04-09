using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandController : MonoBehaviour {

    private PlayerElements playerElements;
    private TouchController touchController;
    public AudioSource firePickup;

    [SerializeField] private Vector3 holdElementsHandRotation;

    [SerializeField] private string cycleTriggerName;
    private bool hasJustCycled = false;

    [SerializeField] private string shootTriggerName;
    private bool hasJustShotElement = false;

    // Start is called before the first frame update
    void Start() {
        playerElements = GetComponent<PlayerElements>();
        touchController = GetComponent<TouchController>();
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

        if (!hasJustShotElement && shootInput == 1) {
            HandleShootElement();
        }
    }

    private void HandleCycle() {
        hasJustCycled = true;
        playerElements.CycleActiveElement();
    }

    public void HandleSwitchToElement() {
        touchController.OffsetRotation = holdElementsHandRotation;
    }

    public void HandleSwitchFromElement() {
        touchController.OffsetRotation = touchController.DefaultRotation;
    }

    private void HandleShootElement() {
        if (playerElements.ActiveElement == null) {
            return;
        }

        hasJustShotElement = true;
        StartCoroutine(playerElements.ShootActiveElement());
    }
}

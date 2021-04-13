using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandController : MonoBehaviour {

    public bool IsCheckingForRightHandInput {
        get => checkForInput != null;
        set {
            if (value) {
                checkForInput = CheckForRightHandInput();
                StartCoroutine(checkForInput);
            }
            else if (IsCheckingForRightHandInput) {
                StopCoroutine(checkForInput);
            }
        }
    }

    private PlayerElements playerElements;
    private TouchController touchController;

    [SerializeField] private Vector3 holdElementsHandRotation;

    [SerializeField] private string cycleTriggerName;
    private bool hasJustCycled = false;

    [SerializeField] private string shootTriggerName;
    private bool hasJustShotElement = false;

    private bool isShootingBlocked = false;

    private IEnumerator checkForInput;

    // Start is called before the first frame update
    void Start() {
        playerElements = GetComponent<PlayerElements>();
        touchController = GetComponent<TouchController>();

        ActionBlocker.AddEnterCallbackToActionBlockers(() => isShootingBlocked = true);
        ActionBlocker.AddExitCallbackToActionBlockers(() => isShootingBlocked = false);
    }

    private IEnumerator CheckForRightHandInput() {
        while (IsCheckingForRightHandInput) {
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

            if (!hasJustShotElement && shootInput == 1 && !isShootingBlocked) {
                HandleShootElement();
            }

            yield return null;
        }
    }

    public void HandleCycle() {
        hasJustCycled = true;
        playerElements.CycleActiveElement();
    }

    public void HandleSwitchToElement() {
        touchController.OffsetRotation = holdElementsHandRotation;
    }

    public void HandleSwitchFromElement() {
        touchController.OffsetRotation = touchController.DefaultRotation;
    }

    public void HandleShootElement() {
        if (playerElements.ActiveElement == null) {
            return;
        }

        hasJustShotElement = true;
        StartCoroutine(playerElements.ShootActiveElement());
    }
}

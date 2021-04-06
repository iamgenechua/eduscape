using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    [Header("Cockpit Door")]

    [SerializeField] private Door cockpitDoor;

    [Header("Ramp")]

    [SerializeField] private BoxCollider rampClosingCollider;

    [SerializeField] private Animator rampAnim;
    [SerializeField] private string rampAnimParam = "isOpen";

    [Header("Summary and Button Stations")]

    [SerializeField] private FadeCanvas[] screenFadeCanvases;
    [SerializeField] private Animator[] buttonCoverAnims;
    [SerializeField] private string buttonCoverAnimParam = "isOpen";

    public bool HasLaunched { get; private set; }

    // Start is called before the first frame update
    void Start() {
        HasLaunched = false;
        cockpitDoor.OpenDoor();
        rampClosingCollider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Launch() {
        // blast off into space
        HasLaunched = true;
        CloseRamp();
        UnlockSummaryAndButtons();
    }

    public void CloseRamp() {
        rampAnim.SetBool(rampAnimParam, false);

        // prevent the player from teleporting out of the ship while the ramp is closing
        rampClosingCollider.gameObject.SetActive(true);
        StartCoroutine(GameManager.Instance.WaitForConditionBeforeAction(
            () => rampAnim.GetCurrentAnimatorStateInfo(0).IsName("Ramp Closed"),
            () => rampClosingCollider.gameObject.SetActive(false)));
    }

    public void UnlockSummaryAndButtons() {
        foreach (FadeCanvas fadeCanvas in screenFadeCanvases) {
            fadeCanvas.FadeIn();
        }

        foreach (Animator anim in buttonCoverAnims) {
            anim.SetBool(buttonCoverAnimParam, true);
        }
    }
}

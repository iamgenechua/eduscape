using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorGate : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string openBooleanParameter;
    [SerializeField] private string openingAnimationTag;
    [SerializeField] private string closingAnimationTag;

    [Tooltip("The action blocker in the negative direction of the x axis.")]
    [SerializeField] private BoxCollider actionBlockerLeft;
    [Tooltip("The action blocker in the positive direction of the x axis.")]
    [SerializeField] private BoxCollider actionBlockerRight;

    public bool IsOpen { get => anim.GetBool(openBooleanParameter); }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        ActivateActionBlocker();
    }

    private void ActivateActionBlocker() {
        Vector3 localPos = transform.InverseTransformPoint(LevelManager.Instance.PlayerBody.transform.position);
        if (localPos.x < 0f) {
            actionBlockerLeft.gameObject.SetActive(false);
            actionBlockerRight.gameObject.SetActive(true);
        } else {
            actionBlockerLeft.gameObject.SetActive(true);
            actionBlockerRight.gameObject.SetActive(false);
        }
    }

    public void Open() {
        actionBlockerLeft.gameObject.SetActive(false);
        actionBlockerRight.gameObject.SetActive(false);
        anim.SetBool(openBooleanParameter, true);
    }

    public void Close() {
        ActivateActionBlocker();
        anim.SetBool(openBooleanParameter, false);
    }

    public bool IsOpeningOrClosing() {
        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return animStateInfo.IsTag(openingAnimationTag) || animStateInfo.IsTag(closingAnimationTag);
    }

    public void Toggle() {
        if (IsOpeningOrClosing()) {
            return;
        }

        if (IsOpen) {
            Close();
        } else {
            Open();
        }
    }
}

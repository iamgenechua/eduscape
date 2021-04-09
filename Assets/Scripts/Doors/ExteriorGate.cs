using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorGate : MonoBehaviour {

    private Animator anim;
    private AudioSource audioSource;

    [Header("Animation")]

    [SerializeField] private string openBooleanParameter;
    [SerializeField] private string openingAnimationTag;
    [SerializeField] private string closingAnimationTag;

    [Header("Audio")]

    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip creakSound;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private AudioClip impactReverbSound;

    [Header("Action Blockers")]

    [Tooltip("The action blocker in the negative direction of the x axis.")]
    [SerializeField] private ActionBlocker actionBlockerLeft;
    [Tooltip("The action blocker in the positive direction of the x axis.")]
    [SerializeField] private ActionBlocker actionBlockerRight;

    public bool IsOpen { get => anim.GetBool(openBooleanParameter); }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        ActivateActionBlocker();
    }

    private void ActivateActionBlocker() {
        Vector3 localPos = transform.InverseTransformPoint(LevelManager.Instance.PlayerBody.transform.position);
        if (localPos.x < 0f) {
            actionBlockerLeft.Deactivate();
            actionBlockerRight.Activate();
        } else {
            actionBlockerLeft.Activate();
            actionBlockerRight.Deactivate();
        }
    }

    public void Open() {
        actionBlockerLeft.Deactivate();
        actionBlockerRight.Deactivate();
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

    public void PlayStartSound() {
        audioSource.PlayOneShot(startSound);
    }

    public void PlayCreakSouund() {
        audioSource.PlayOneShot(creakSound);
    }

    public void PlayImpactSound() {
        audioSource.PlayOneShot(impactSound);
    }

    public void PlayImpactReverbSound() {
        audioSource.PlayOneShot(impactReverbSound);
    }
}

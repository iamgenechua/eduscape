using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorGate : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string openBooleanParameter;
    [SerializeField] private string openingAnimationTag;
    [SerializeField] private string closingAnimationTag;

    public bool IsOpen { get => anim.GetBool(openBooleanParameter); }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Open() {
        anim.SetBool(openBooleanParameter, true);
    }

    public void Close() {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    [SerializeField] private FadeCanvas[] screenFadeCanvases;
    [SerializeField] private Animator[] buttonCoverAnims;
    [SerializeField] private string buttonCoverAnimParam = "isOpen";

    public bool HasLaunched { get; private set; }

    // Start is called before the first frame update
    void Start() {
        HasLaunched = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Launch() {
        // blast off into space
        HasLaunched = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour {

    [SerializeField] private FadeCanvas rationaleSummaryFadeCanvas;
    [SerializeField] private FadeCanvas restartFadeCanvas;
    [SerializeField] private FadeCanvas quitFadeCanvas;

    [SerializeField] private Animator restartButtonCoverAnim;
    [SerializeField] private Animator quitButtonCoverAnim;
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
        rationaleSummaryFadeCanvas.FadeIn();
        restartFadeCanvas.FadeIn();
        quitFadeCanvas.FadeIn();
        restartButtonCoverAnim.SetBool(buttonCoverAnimParam, true);
        quitButtonCoverAnim.SetBool(buttonCoverAnimParam, true);
    }
}

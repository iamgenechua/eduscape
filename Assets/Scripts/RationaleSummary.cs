using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RationaleSummary : FadeCanvas {

    [SerializeField] private string[] rationaleTexts;
    private int currTextIndex;

    void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        currTextIndex = 0;
        text.text = rationaleTexts[currTextIndex];
    }

    public void DisplayPrevious() {
        if (!IsFadedIn) {
            return;
        }

        currTextIndex = currTextIndex == 0 ? rationaleTexts.Length - 1 : currTextIndex - 1;
        text.text = rationaleTexts[currTextIndex];
    }

    public void DisplayNext() {
        if (!IsFadedIn) {
            return;
        }

        currTextIndex = currTextIndex == rationaleTexts.Length - 1 ? 0 : currTextIndex + 1;
        text.text = rationaleTexts[currTextIndex];
    }
}

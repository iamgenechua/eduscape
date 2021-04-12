using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RationaleSummary : MonoBehaviour {

    [SerializeField] private TextMeshPro text;
    [SerializeField] private FadeText rationaleSummaryFadeCanvas;

    private string[] rationaleTexts = new string[] {
        Rationale.Conduction,
        Rationale.StuckDoor,
        Rationale.MoleculeContainer,
        Rationale.PistonPuzzle,
        Rationale.PressurePuzzle
    };

    private int currTextIndex;

    // Start is called before the first frame update
    void Start() {
        currTextIndex = 0;
        text.text = rationaleTexts[currTextIndex];
    }

    public void DisplayPrevious() {
        if (!rationaleSummaryFadeCanvas.IsFadedIn) {
            return;
        }

        currTextIndex = currTextIndex == 0 ? rationaleTexts.Length - 1 : currTextIndex - 1;
        text.text = rationaleTexts[currTextIndex];
    }

    public void DisplayNext() {
        if (!rationaleSummaryFadeCanvas.IsFadedIn) {
            return;
        }

        currTextIndex = currTextIndex == rationaleTexts.Length - 1 ? 0 : currTextIndex + 1;
        text.text = rationaleTexts[currTextIndex];
    }
}

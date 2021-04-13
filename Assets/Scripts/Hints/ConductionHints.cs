using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConductionHints : HintsController {

    [SerializeField] protected DisplayScreen[] hintScreens;
    [SerializeField] protected float fontSize;
    [SerializeField] protected float originalFontSize;

    protected IEnumerator[] hintsDisplaying;

    protected override void Start() {
        base.Start();
        hintsDisplaying = new IEnumerator[hintScreens.Length];
    }

    protected override void ActivateHints() {
        base.ActivateHints();
        for (int i = 0; i < hintScreens.Length; i++) {
            hintsDisplaying[i] = DisplayHint(hintScreens[i]);
            StartCoroutine(hintsDisplaying[i]);
        }
    }

    protected IEnumerator DisplayHint(DisplayScreen screen) {
        TextMeshPro screenText = screen.GetComponentInChildren<TextMeshPro>();

        string originalText = screenText.text;
        screen.SetText("", false);

        screenText.fontSize = fontSize;

        screen.SetText(hintText);
        yield return new WaitUntil(() => !screen.IsRollingOut);

        yield return new WaitForSeconds(5f);

        screen.SetText("", false);

        screenText.fontSize = originalFontSize;
        screen.SetText(originalText);
    }

    public override void DeactivateHints() {
        base.DeactivateHints();
        for (int i = 0; i < hintsDisplaying.Length; i++) {
            if (hintsDisplaying[i] != null) {
                hintScreens[i].SetText("", false, true);
                hintScreens[i].GetComponentInChildren<TextMeshPro>().fontSize = originalFontSize;
                StopCoroutine(hintsDisplaying[i]);
            }
        }
    }
}

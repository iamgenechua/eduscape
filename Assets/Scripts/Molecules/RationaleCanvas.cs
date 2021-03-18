using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RationaleCanvas : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float waitTimeBeforeFadeIn;
    [SerializeField] private float fadeInLength;

    public void FadeIn() {
        StartCoroutine(FadeIn(fadeInLength));
    }

    private IEnumerator FadeIn(float timeTaken) {
        yield return new WaitForSeconds(waitTimeBeforeFadeIn);

        Color color = text.color;

        while (text.color.a < 0.99f) {
            color.a += Time.deltaTime / timeTaken;
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }
}

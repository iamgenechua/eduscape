using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hintsConduction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float waitTimeBeforeFadeIn;
    [SerializeField] private float fadeInLength;

    public void FadeIn() {
        StartCoroutine(FadeIn(fadeInLength));
    }

    private IEnumerator FadeIn(float timeTaken) {
        yield return new WaitForSeconds(waitTimeBeforeFadeIn);
    }
}

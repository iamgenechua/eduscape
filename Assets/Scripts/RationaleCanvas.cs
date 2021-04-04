using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RationaleCanvas : MonoBehaviour {

    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected float waitTimeBeforeFadeIn;
    [SerializeField] protected float fadeInLength;

    public bool IsFadedIn { get; protected set; }

    protected virtual void Start() {
        IsFadedIn = false;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    public void FadeIn() {
        StartCoroutine(FadeIn(fadeInLength));
    }

    protected IEnumerator FadeIn(float timeTaken) {
        yield return new WaitForSeconds(waitTimeBeforeFadeIn);

        Color color = text.color;

        while (text.color.a < 0.99f) {
            color.a += Time.deltaTime / timeTaken;
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
        IsFadedIn = true;
    }
}

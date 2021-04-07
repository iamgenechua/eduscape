using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour {

    protected TextMeshPro text;
    
    [SerializeField] protected float waitTimeBeforeFade = 1f;
    [SerializeField] protected float fadeLength = 3f;

    public bool IsFadedIn { get; protected set; }

    protected virtual void Awake() {
        text = GetComponent<TextMeshPro>();
    }

    protected virtual void Start() {
        IsFadedIn = false;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    public void FadeIn() {
        if (IsFadedIn) {
            return;
        }

        StartCoroutine(FadeIn(fadeLength));
    }

    public void FadeOut() {
        if (!IsFadedIn) {
            return;
        }

        StartCoroutine(FadeOut(fadeLength));
    }

    protected IEnumerator FadeIn(float timeTaken) {
        yield return new WaitForSeconds(waitTimeBeforeFade);

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

    protected IEnumerator FadeOut(float timeTaken) {
        yield return new WaitForSeconds(waitTimeBeforeFade);

        Color color = text.color;

        while (text.color.a > 0.01f) {
            color.a -= Time.deltaTime / timeTaken;
            text.color = color;
            yield return null;
        }

        color.a = 0f;
        text.color = color;
        IsFadedIn = false;
    }
}

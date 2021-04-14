using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HintsController : MonoBehaviour {

    [SerializeField] protected string hintText;
    [SerializeField] protected float hintCountdownTime = 60f;
    [SerializeField] protected AudioSource[] hintAlertAudioSources;

    public bool IsPuzzleSolved { get; protected set; }
    public bool AreHintsProvided { get; protected set; }
    public bool IsCountingDown { get; protected set; }

    protected IEnumerator countdown;

    protected virtual void Start() {}

    protected IEnumerator CountDownToHintsActivation() {
        IsCountingDown = true;

        yield return new WaitForSeconds(hintCountdownTime);

        IsCountingDown = false;
        AreHintsProvided = true;

        ActivateHints();
    }

    protected virtual void ActivateHints() {
        foreach (AudioSource source in hintAlertAudioSources) {
            source.Play();
        }
    }

    public virtual void DeactivateHints() {
        IsPuzzleSolved = true;
        if (countdown != null) {
            StopCoroutine(countdown);
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (!IsPuzzleSolved && !AreHintsProvided && !IsCountingDown && other.CompareTag("Player")) {
            countdown = CountDownToHintsActivation();
            StartCoroutine(countdown);
        }
    }
}

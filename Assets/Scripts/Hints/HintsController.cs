using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HintsController : MonoBehaviour {

    [SerializeField] protected float hintCountdownTime = 60f;

    [SerializeField] protected AudioSource hintAlertAudioSource;

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

    protected abstract void ActivateHints();

    public virtual void DeactivateHints() {
        StopCoroutine(countdown);
    }

    protected void OnTriggerEnter(Collider other) {
        if (!AreHintsProvided && !IsCountingDown && other.CompareTag("Player")) {
            countdown = CountDownToHintsActivation();
            StartCoroutine(countdown);
        }
    }
}

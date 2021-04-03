using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fade : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string fadeInAnimParam;
    [SerializeField] private string fadeOutAnimParam;

    [SerializeField] private UnityEvent fadeInCompleteEvent;
    public UnityEvent FadeInCompleteEvent { get => fadeInCompleteEvent; }

    [SerializeField] private UnityEvent fadeOutCompleteEvent;
    public UnityEvent FadeOutCompleteEvent { get => fadeOutCompleteEvent; }

    public bool IsFading { get; private set; }

    void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
    }

    public void FadeIn() {
        IsFading = true;
        anim.SetTrigger(fadeInAnimParam);
    }

    public void FadeInComplete() {
        IsFading = false;
        fadeInCompleteEvent.Invoke();
    }

    public void FadeOut() {
        IsFading = true;
        anim.SetTrigger(fadeOutAnimParam);
    }

    public void FadeOutComplete() {
        IsFading = false;
        fadeOutCompleteEvent.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPickup : Element {

    protected AudioSource audioSource;

    [SerializeField] protected AudioClip pickUpSound;
    public AudioClip PickUpSound { get => pickUpSound; }

    [Tooltip("The held element corresponding to this pickup element.")]
    [SerializeField] protected ElementHeld correspondingHeldElement;
    public ElementHeld CorrespondingHeldElement { get => correspondingHeldElement; }

    [Space(10)]

    [SerializeField] protected ElementEvent elementPickedUpEvent;

    protected override void Awake() {
        base.Awake();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        audioSource.Play();
    }

    public void PickUp() {
        elementPickedUpEvent.Invoke(this);
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hand")) {
            PickUp();
        }
    }
}

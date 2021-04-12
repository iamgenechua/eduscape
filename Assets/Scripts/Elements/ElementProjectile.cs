using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementProjectile : Element {

    protected AudioSource audioSource;

    protected override void Awake() {
        base.Awake();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    protected override void Start() {
        base.Start();
        // audioSource.Play();
    }

    protected void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Hand") && !collision.gameObject.CompareTag("Player")) {
            ElementTarget elementTarget = collision.gameObject.GetComponent<ElementTarget>();
            if (elementTarget != null) {
                elementTarget.GetHitByElementProjectile(this);
            }

            Destroy(gameObject);
        }
    }
}

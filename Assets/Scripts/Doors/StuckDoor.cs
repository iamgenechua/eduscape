using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckDoor : Door {

    [SerializeField] protected AudioClip openAttemptClip;
    [SerializeField] protected AudioClip closeAttemptClip;

    protected override void Start() {
        base.Start();
        audioSource.clip = closeAttemptClip;
    }

    public void PlayOpenAttemptSound() {
        audioSource.clip = openAttemptClip;
        audioSource.Play();
    }

    public void PlayCloseAttemptSound() {
        audioSource.clip = closeAttemptClip;
        audioSource.Play();
    }

    public override void OpenDoor() {
        base.OpenDoor();
        audioSource.clip = openSound;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    public AudioSource openAudioSource;
    public AudioSource closeAudioSource;

    [SerializeField] private string animOpenBool;

    [Tooltip("Object, with colliders, to activate when door is closed.")]
    [SerializeField] private GameObject closedColliders;

    public bool IsOpen { get => anim.GetBool(animOpenBool); private set => anim.SetBool(animOpenBool, value); }

    private bool isPlayerInDoorway = false;
    public bool IsPlayerInDoorway { get => isPlayerInDoorway; }
    
    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        closedColliders.SetActive(!anim.GetBool(animOpenBool));
    }

    public void OpenDoor() {
        if (IsOpen) {
            return;
        }

        IsOpen = true;
        closedColliders.SetActive(false);
        openAudioSource.Play();
    }

    public void CloseDoor() {
        if (!IsOpen) {
            return;
        }

        IsOpen = false;
        closedColliders.SetActive(true);
        closeAudioSource.Play();
    }

    public void ToggleDoor(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            if (IsOpen) {
                CloseDoor();
            } else {
                OpenDoor();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = false;
        }
    }
}

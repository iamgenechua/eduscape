using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string animOpenBool;

    [Tooltip("Object, with colliders, to activate when door is closed.")]
    [SerializeField] private GameObject closedColliders;

    public bool IsOpen { get => anim.GetBool(animOpenBool); private set => anim.SetBool(animOpenBool, value); }

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
    }

    public void CloseDoor() {
        if (!IsOpen) {
            return;
        }

        IsOpen = false;
        closedColliders.SetActive(true);
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
}

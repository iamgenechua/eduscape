using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string animOpenBool;

    [Tooltip("Object, with colliders, to activate when door is closed.")]
    [SerializeField] private GameObject closedColliders;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        closedColliders.SetActive(!anim.GetBool(animOpenBool));
    }

    public void OpenDoor() {
        anim.SetBool(animOpenBool, true);
        closedColliders.SetActive(false);
    }

    public void CloseDoor() {
        anim.SetBool(animOpenBool, false);
        closedColliders.SetActive(true);
    }

    public void OpenDoor(Element element) {
        if (element.ElementType == Element.Type.WATER || element.ElementType == Element.Type.AIR) {
            OpenDoor();
        }
    }
}

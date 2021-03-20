using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor() {
        anim.SetBool("isOpen", true);
    }

    public void CloseDoor() {
        anim.SetBool("isOpen", false);
    }

    public void OpenDoor(Element element) {
        if (element.ElementType == Element.Type.WATER || element.ElementType == Element.Type.AIR) {
            OpenDoor();
        }
    }
}

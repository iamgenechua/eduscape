using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    [SerializeField] protected Door door;

    // Start is called before the first frame update
    protected virtual void Start() {
        
    }

    // Update is called once per frame
    protected virtual void Update() {
        
    }

    public void ToggleDoor(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            if (door.IsOpen) {
                door.CloseDoor();
            } else {
                door.OpenDoor();
            }
        }
    }
}

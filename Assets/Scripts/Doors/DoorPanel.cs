using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    [SerializeField] protected Door door;

    [SerializeField] protected MeshRenderer mesh;
    [SerializeField] protected Material openMaterial;
    [SerializeField] protected Material closedMaterial;
    [SerializeField] protected Material jamMaterial;

    [SerializeField] private float jamDuration = 0.5f;
    private bool isJammed = false; // jammed when player is standing in doorway

    // Start is called before the first frame update
    protected virtual void Start() {

    }

    // Update is called once per frame
    protected virtual void Update() {
        
    }

    /// <summary>
    /// Toggles the door if the given element is not fire.
    /// </summary>
    /// <param name="element">The element that contacted this door panel.</param>
    public void ToggleDoor(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            ToggleDoor();
        }
    }

    /// <summary>
    /// Toggles the door.
    /// </summary>
    protected void ToggleDoor() {
        if (isJammed) {
            return;
        }

        if (door.IsOpen) {
            if (door.IsPlayerInDoorway) {
                StartCoroutine(Jam());
            } else {
                Deactivate();
                door.CloseDoor();
            }
        } else {
            Activate();
            door.OpenDoor();
        }
    }

    protected void Activate() {
        mesh.material = openMaterial;
    }

    protected void Deactivate() {
        mesh.material = closedMaterial;
    }

    protected IEnumerator Jam() {
        isJammed = true;
        mesh.material = jamMaterial;
        yield return new WaitForSeconds(jamDuration);
        mesh.material = openMaterial;
        isJammed = false;
    }
}

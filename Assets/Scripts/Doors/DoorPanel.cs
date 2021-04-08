using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    [SerializeField] protected Door door;

    [SerializeField] protected MeshRenderer mesh;
    [SerializeField] protected Material openMaterial;
    [SerializeField] protected Material closedMaterial;
    [SerializeField] protected Material jamMaterial;
    [SerializeField] protected Material offMaterial;

    [SerializeField] private float jamDuration = 0.5f;
    protected bool isJammed = false; // jammed when player is standing in doorway

    protected Material materialBeforeSwitchedOff;
    protected bool isOff = false;

    // Start is called before the first frame update
    protected virtual void Start() {
        materialBeforeSwitchedOff = closedMaterial;
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
        if (isJammed || isOff || door.IsOpeningOrClosing()) {
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

    public void SwitchOff() {
        isOff = true;
        materialBeforeSwitchedOff = mesh.material;
        mesh.material = offMaterial;
    }

    public void SwitchOn() {
        isOff = false;
        mesh.material = materialBeforeSwitchedOff;
    }
}

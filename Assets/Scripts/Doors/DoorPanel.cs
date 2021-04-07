using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    public AudioSource activatingSound;
    public AudioSource deactivatingSound;
    public AudioSource jammingSound;

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
        if (isJammed || isOff) {
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
        deactivatingSound.Stop();
        jammingSound.Stop();
        activatingSound.Play();
        mesh.material = openMaterial;
    }

    protected void Deactivate() {
        jammingSound.Stop();
        activatingSound.Stop();
        deactivatingSound.Play();
        mesh.material = closedMaterial;
    }

    protected IEnumerator Jam() {
        deactivatingSound.Stop();
        activatingSound.Stop();
        jammingSound.Play();
        isJammed = true;
        mesh.material = jamMaterial;
        yield return new WaitForSeconds(jamDuration);
        mesh.material = openMaterial;
        isJammed = false;
    }

    public void SwitchOff() {
        materialBeforeSwitchedOff = mesh.material;
        mesh.material = offMaterial;
    }

    public void SwitchOn() {
        mesh.material = materialBeforeSwitchedOff;
    }
}

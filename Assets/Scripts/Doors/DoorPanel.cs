using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    public enum Status { ACTIVATED, DEACTIVATED, JAMMED, OFF }
    protected Status status;

    [SerializeField] protected Door door;

    [SerializeField] protected float jamDuration = 0.5f;

    [Header("Materials")]

    [SerializeField] protected MeshRenderer mesh;
    [SerializeField] protected Material openMaterial;
    [SerializeField] protected Material closedMaterial;
    [SerializeField] protected Material jamMaterial;
    [SerializeField] protected Material offMaterial;

    protected Material materialBeforeSwitchedOff;

    [Header("Audio")]

    [SerializeField] protected AudioClip activateSound;
    [SerializeField] protected AudioClip deactivateSound;
    [SerializeField] protected AudioClip jamSound;

    protected AudioSource audioSource;

    protected virtual void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        status = Status.DEACTIVATED;
        materialBeforeSwitchedOff = closedMaterial;
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
        if (status == Status.JAMMED || status == Status.OFF || door.IsOpeningOrClosing()) {
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
        status = Status.ACTIVATED;
        mesh.material = openMaterial;
        audioSource.clip = activateSound;
        audioSource.Play();
    }

    protected void Deactivate() {
        status = Status.DEACTIVATED;
        mesh.material = closedMaterial;
        audioSource.clip = deactivateSound;
        audioSource.Play();
    }

    protected IEnumerator Jam() {
        Status originalStatus = status;

        status = Status.JAMMED;
        mesh.material = jamMaterial;
        audioSource.clip = jamSound;
        audioSource.Play();

        yield return new WaitForSeconds(jamDuration);
        
        mesh.material = openMaterial;
        status = originalStatus;
    }

    public void SwitchOff() {
        status = Status.OFF;
        materialBeforeSwitchedOff = mesh.material;
        mesh.material = offMaterial;
    }

    public void SwitchOn() {
        status = door.IsOpen ? Status.ACTIVATED : Status.DEACTIVATED;
        mesh.material = materialBeforeSwitchedOff;
    }
}

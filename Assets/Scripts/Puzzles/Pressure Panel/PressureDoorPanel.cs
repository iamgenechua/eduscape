using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureDoorPanel : DoorPanel {

    [Tooltip("Two consecutive molecules must hit within this time frame to maintain pressure.")]
    [SerializeField] private float hitWindow = 0.5f;

    private float lastMoleculeHitTime = 0f;

    [Tooltip("The amount of time pressure needs to stay on the panel to open the door.")]
    [SerializeField] private float pressureDurationNeeded = 3f;

    private float pressureOnStartTime = 0f;
    private bool isPressureOn = false;

    [SerializeField] private TextRollout rationale;

    protected override void Start() {
        base.Start();
        StartCoroutine(WaitForMoleculesToOpenDoor());
    }

    private IEnumerator WaitForMoleculesToOpenDoor() {
        while (!door.IsOpen) {
            if (status == Status.ACTIVATED && Time.time - lastMoleculeHitTime > hitWindow) {
                isPressureOn = false;
                Deactivate();
            }

            if (isPressureOn & Time.time - pressureOnStartTime >= pressureDurationNeeded) {
                door.OpenDoor();
                rationale.StartRollOut(Rationale.PressurePuzzle);
            }

            yield return null;
        }
    }

    private void HandleMoleculeHit() {
        if (!isPressureOn) {
            isPressureOn = true;
            pressureOnStartTime = Time.time;
            Activate();
        }

        lastMoleculeHitTime = Time.time;

        audioSource.clip = activateSound;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision) {
        if (!door.IsOpen && collision.gameObject.GetComponent<Molecule>()) {
            HandleMoleculeHit();
        }
    }
}

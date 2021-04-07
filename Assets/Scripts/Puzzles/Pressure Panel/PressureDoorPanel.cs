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

    private bool isDoorOpen = false;

    [SerializeField] private FadeText rationaleCanvas;

    // Start is called before the first frame update
    protected override void Start() {
        
    }

    // Update is called once per frame
    protected override void Update() {
        if (!isDoorOpen && Time.time - lastMoleculeHitTime > hitWindow) {
            isPressureOn = false;
            Deactivate();
        }

        if (isPressureOn && Time.time - pressureOnStartTime >= pressureDurationNeeded) {
            isDoorOpen = true;
            door.OpenDoor();
            rationaleCanvas.FadeIn();
        }
    }

    private void HandleMoleculeHit() {
        if (!isPressureOn) {
            isPressureOn = true;
            pressureOnStartTime = Time.time;
            Activate();
        }

        lastMoleculeHitTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision) {
        if (!isDoorOpen && collision.gameObject.GetComponent<Molecule>()) {
            HandleMoleculeHit();
        }
    }
}

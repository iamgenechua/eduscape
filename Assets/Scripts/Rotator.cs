using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public enum RotationDirection { CLOCKWISE, ANTICLOCKWISE, FORWARD, BACK }

    [Tooltip("The pivot point about which to rotate. If none is given, it is set to the object's transform.")]
    [SerializeField] private Transform pivot;
    
    [SerializeField] private float speed;

    [SerializeField] private RotationDirection rotationAxis = RotationDirection.ANTICLOCKWISE;

    void Start() {
        if (pivot == null) {
            pivot = transform;
        }
    }

    // Update is called once per frame
    void Update() {
        switch (rotationAxis) {
            case RotationDirection.CLOCKWISE:
                transform.RotateAround(pivot.position, Vector3.up, speed * Time.deltaTime);
                return;
            case RotationDirection.ANTICLOCKWISE:
                transform.RotateAround(pivot.position, Vector3.down, speed * Time.deltaTime);
                return;
            case RotationDirection.FORWARD:
                transform.RotateAround(pivot.position, Vector3.back, speed * Time.deltaTime);
                return;
            case RotationDirection.BACK:
                transform.RotateAround(pivot.position, Vector3.forward, speed * Time.deltaTime);
                return;
        }
    }
}

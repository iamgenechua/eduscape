using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private Vector3 movementDirection;
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
        rb.velocity = movementDirection * speed;
        rb.angularVelocity = rotationDirection * rb.velocity.magnitude * Mathf.Deg2Rad;
    }

    private void StartExplosions() {

    }

    private void OnCollisionEnter(Collision collision) {
        StartExplosions();
    }
}

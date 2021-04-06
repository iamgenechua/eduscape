using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTarget : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private float turnTimeMultiplier = 20f;

    [SerializeField] private Transform[] waypoints;
    private int currWaypoint = 0;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        if (isMoving) {
            if (Vector3.Distance(transform.position, waypoints[currWaypoint].position) < 0.1f) {
                currWaypoint++;
                if (currWaypoint >= waypoints.Length) {
                    isMoving = false;
                    return;
                }
            }

            Transform dest = waypoints[currWaypoint];
            Vector3 toDest = (dest.position - rb.position).normalized;
            Vector3 velocityDirection = Vector3.Slerp(transform.forward, toDest, Time.time / turnTimeMultiplier);
            rb.velocity = velocityDirection * rb.velocity.magnitude;
        }
    }

    public void StartMoving() {
        rb.velocity = transform.forward * 5f;
        isMoving = true;
    }
}

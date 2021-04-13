using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    private BoxCollider bodyCollider;

    private void Awake() {
        bodyCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Teleports the attached Transform towards the given position.
    /// </summary>
    /// <param name="targetPos">The target teleport Vector3 position.</param>
    public void TeleportTo(Vector3 targetPos) {
        // find the closest point on the body in the direction of the target position
        Vector3 closestPoint = bodyCollider.ClosestPointOnBounds(targetPos);
        // get the distance from that closest point to the target position
        float distanceToTravel = Vector3.Distance(closestPoint, targetPos);
        // move to the adjusted target position
        transform.parent.position += (targetPos - transform.position).normalized * distanceToTravel;
    }
}

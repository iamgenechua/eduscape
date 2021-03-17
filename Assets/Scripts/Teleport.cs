using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    /// <summary>
    /// Teleports the attached Transform to the given position.
    /// </summary>
    /// <param name="targetPos">The target teleport Vector3 position.</param>
    public void TeleportTo(Vector3 targetPos) {
        transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
    }
}

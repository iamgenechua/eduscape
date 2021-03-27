using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonPlatform : MonoBehaviour {

    [SerializeField] private float maxLocalY;
    [SerializeField] private float minLocalY;

    public float GetMoveableDistance() {
        return maxLocalY - minLocalY;
    }

    private bool IsWithinMovementLimits() {
        return transform.localPosition.y >= minLocalY && transform.localPosition.y <= maxLocalY;
    }

    public void Move(float speed) {
        if (!IsWithinMovementLimits()) {
            return;
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}

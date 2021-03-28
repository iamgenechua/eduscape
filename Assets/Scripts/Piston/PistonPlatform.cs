using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PistonPlatform : MonoBehaviour {

    [SerializeField] private float minLocalY;
    [SerializeField] private float maxLocalY;

    [SerializeField] private UnityEvent reachTopEvent;
    [SerializeField] private UnityEvent reachBottomEvent;

    private float currSpeed = 0f;
    private bool isMoving = false;

    public bool HasReachedTop { get => transform.localPosition.y > maxLocalY; }
    public bool HasReachedBottom { get => transform.localPosition.y < minLocalY; }

    void Update() {
        if (isMoving) {
            Move(currSpeed);
        }
    }

    public float GetMoveableDistance() {
        return maxLocalY - minLocalY;
    }

    public void StartMoving() {
        isMoving = true;
    }

    public void StopMoving() {
        isMoving = false;
    }

    public void ChangeSpeed(float speed) {
        if (!isMoving) {
            StartMoving();
        }

        currSpeed = speed;
    }

    private void Move(float speed) {
        if (HasReachedTop) {
            reachTopEvent.Invoke();
        } else if (HasReachedBottom) {
            reachBottomEvent.Invoke();
        }

        if (HasReachedTop || HasReachedBottom) {
            StopMoving();
            return;
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
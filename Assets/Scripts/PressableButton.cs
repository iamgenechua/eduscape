using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableButton : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private float pressLength;
    [SerializeField] private float releaseSpeed;
    private Vector3 startLocalPos;
    private float startHeight;

    private float releaseVelocity = 0f;

    public bool IsPressed { get; private set; }
    private bool isPressing = false;

    [SerializeField] private UnityEvent pressedEvent;
    [SerializeField] private UnityEvent releasedEvent;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        startLocalPos = transform.localPosition;
        startHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update() {
        // lock x and z values of local position
        transform.localPosition = new Vector3(startLocalPos.x, transform.localPosition.y, startLocalPos.z);

        float distanceFromStartPos = Mathf.Abs(transform.localPosition.y - startHeight);

        if (isPressing) {
            if (distanceFromStartPos >= pressLength) {
                transform.localPosition = new Vector3(transform.localPosition.x, startHeight - pressLength, transform.localPosition.z);
                rb.velocity = Vector3.zero;
                if (!IsPressed) {
                    IsPressed = true;
                    pressedEvent.Invoke();
                }
            }
        }

        if (!isPressing) {
            if (distanceFromStartPos > 0.01f) {
                float direction = transform.localPosition.y < startHeight ? 1 : -1;
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y + direction * releaseSpeed * Time.deltaTime,
                    transform.localPosition.z);
            } else {
                transform.localPosition = new Vector3(transform.localPosition.x, startHeight, transform.localPosition.z);
                rb.velocity = Vector3.zero;
            }

            if (IsPressed && distanceFromStartPos < pressLength / 2f) {
                // we consider the button released if it's more than half way up
                IsPressed = false;
                releasedEvent.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        isPressing = true;
    }

    private void OnCollisionExit(Collision collision) {
        isPressing = false;
    }
}

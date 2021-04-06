using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableButton : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private float pressLength;
    [SerializeField] private float releaseSpeed;
    private float startHeight;

    public bool IsPressed { get; private set; }
    private bool isPressing = false;

    [SerializeField] private UnityEvent pressedEvent;
    [SerializeField] private UnityEvent releasedEvent;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        startHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update() {
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
                transform.Translate(Vector3.up * releaseSpeed * Time.deltaTime, Space.Self);
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

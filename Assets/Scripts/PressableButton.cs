using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableButton : MonoBehaviour {

    [SerializeField] private float pressLength;
    private Vector3 startPos;

    public bool IsPressed { get; private set; }

    [SerializeField] private UnityEvent pressedEvent;
    [SerializeField] private UnityEvent releasedEvent;

    // Start is called before the first frame update
    void Start() {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        float distanceFromStartPos = Mathf.Abs(transform.position.y - startPos.y);
        if (distanceFromStartPos >= pressLength) {
            transform.position = new Vector3(transform.position.x, startPos.y - pressLength, transform.position.z);
            if (!IsPressed) {
                IsPressed = true;
                pressedEvent.Invoke();
            }
        } else if (IsPressed && distanceFromStartPos < pressLength / 2f) {
            // we consider the button released if it's more than half way up
            IsPressed = false;
            releasedEvent.Invoke();
        }

        if (transform.position.y > startPos.y) {
            transform.position = startPos;
        }
    }
}

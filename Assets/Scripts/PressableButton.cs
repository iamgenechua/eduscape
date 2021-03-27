using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableButton : MonoBehaviour {

    [SerializeField] private float pressLength;
    private Vector3 startPos;
    private bool isPressed = false;

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
            if (!isPressed) {
                isPressed = true;
                pressedEvent.Invoke();
            }
        } else if (isPressed) {
            isPressed = false;
            releasedEvent.Invoke();
        }

        if (transform.position.y > startPos.y) {
            transform.position = startPos;
        }
    }
}

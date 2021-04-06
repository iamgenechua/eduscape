using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableButton : MonoBehaviour {

    public AudioSource pressAudioSource;
    public AudioSource releaseAudioSource;

    [SerializeField] private float pressLength;
    private Vector3 startPos;

    public bool IsPressed { get; private set; }

    [SerializeField] private UnityEvent pressedEvent;
    [SerializeField] private UnityEvent releasedEvent;

    void Awake() {
	    pressAudioSource = GetComponentInChildren<AudioSource>();
        releaseAudioSource = GetComponentInChildren<AudioSource>();
    }


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
                pressAudioSource.Play(); // play button pressed
            }
        } else if (IsPressed && distanceFromStartPos < pressLength / 2f) {
            // we consider the button released if it's more than half way up
            IsPressed = false;
            releasedEvent.Invoke();
            releaseAudioSource.Play(); // play button release
        }

        if (transform.position.y > startPos.y) {
            transform.position = startPos;
        }
    }
}

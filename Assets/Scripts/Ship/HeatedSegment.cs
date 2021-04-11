using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatedSegment : MonoBehaviour {

    public enum State { COOLED, COOLING, HEATED, HEATING }
    public State CurrState { get; private set; }

    private Transform pivot;

    private enum ScaleAxis { X, Y, Z }
    [SerializeField] private ScaleAxis scaleAxis;

    [SerializeField] private float scaleSpeed = 0.4f;

    [SerializeField] private AudioSource heatingAudioSource;
    [SerializeField] private AudioSource heatedAudioSource;

    void Awake() {
        pivot = transform.parent;
    }

    // Start is called before the first frame update
    void Start() {
        switch (scaleAxis) {
            case ScaleAxis.X:
                pivot.localScale = new Vector3(0f, pivot.localScale.y, pivot.localScale.z);
                break;
            case ScaleAxis.Y:
                pivot.localScale = new Vector3(pivot.localScale.x, 0f, pivot.localScale.z);
                break;
            case ScaleAxis.Z:
                pivot.localScale = new Vector3(pivot.localScale.x, pivot.localScale.y, 0f);
                break;
        }

        CurrState = State.COOLED;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public IEnumerator Heat(float percentage) {
        CurrState = State.HEATING;
        gameObject.SetActive(true);

        float currScaleValue = 0f;
        while (currScaleValue < percentage) {
            pivot.localScale += GetScaleAxisUnitVector() * scaleSpeed * Time.deltaTime;
            currScaleValue = GetScaleAxisValue();
            yield return null;
        }

        CurrState = State.HEATED;
    }

    public IEnumerator Cool() {
        CurrState = State.COOLING;

        float currSpeed = 0.01f;
        float currScaleValue = GetScaleAxisValue();

        while (currScaleValue > 0.01f) {
            pivot.localScale -= GetScaleAxisUnitVector() * scaleSpeed * Time.deltaTime;
            currScaleValue = GetScaleAxisValue();

            if (currSpeed < scaleSpeed) {
                currSpeed *= 1.01f;
            }

            yield return null;
        }

        gameObject.SetActive(false);

        CurrState = State.COOLED;
    }

    private float GetScaleAxisValue() {
        switch (scaleAxis) {
            case ScaleAxis.X:
                return pivot.localScale.x;
            case ScaleAxis.Y:
                return pivot.localScale.y;
            case ScaleAxis.Z:
                return pivot.localScale.z;
            default:
                throw new System.ArgumentException($"{scaleAxis} has no corresponding value.");
        }
    }

    private Vector3 GetScaleAxisUnitVector() {
        switch (scaleAxis) {
            case ScaleAxis.X:
                return Vector3.right;
            case ScaleAxis.Y:
                return Vector3.up;
            case ScaleAxis.Z:
                return Vector3.forward;
            default:
                throw new System.ArgumentException($"{scaleAxis} has no corresponding unit vector.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipEngineConnectorSegment : MonoBehaviour {

    /// <summary>
    /// The percentage of energy lost after heating a segment of the given state.
    /// </summary>
    private static readonly Dictionary<State, float> stateEnergyLostDict = new Dictionary<State, float>() {
        [State.BASE] = 0.5f,
        [State.METAL] = 0f,
        [State.WATER] = 1f
    };

    private static readonly float heatedVisualScaleSpeed = 0.4f;

    public enum State { BASE, METAL, WATER }

    private State state = State.BASE;

    public enum HeatedSegmentScaleAxis { X, Y, Z }

    [SerializeField] private Transform heatedSegmentVisual;
    [SerializeField] private HeatedSegmentScaleAxis heatedVisualScaleAxis;

    private MeshRenderer mesh;

    public bool IsHeated { get; private set; }

    [SerializeField] private ShipEngineConnectorSegment[] connectedSegments;
    [SerializeField] private ShipEngine engine;

    void Awake() {
        mesh = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
        switch (heatedVisualScaleAxis) {
            case HeatedSegmentScaleAxis.X:
                heatedSegmentVisual.localScale = new Vector3(0f, heatedSegmentVisual.localScale.y, heatedSegmentVisual.localScale.z);
                break;
            case HeatedSegmentScaleAxis.Y:
                heatedSegmentVisual.localScale = new Vector3(heatedSegmentVisual.localScale.x, 0f, heatedSegmentVisual.localScale.z);
                break;
            case HeatedSegmentScaleAxis.Z:
                heatedSegmentVisual.localScale = new Vector3(heatedSegmentVisual.localScale.x, heatedSegmentVisual.localScale.y, 0f);
                break;
        }

        heatedSegmentVisual.gameObject.SetActive(false);
    }

    public void ChangeState(State newState, Material newMaterial) {
        state = newState;
        mesh.material = newMaterial;
    }

    public void Heat(float currEnergy, float totalEnergy, UnityAction failureCallback) {
        if (IsHeated) {
            return;
        }

        StartCoroutine(StartHeating(currEnergy, totalEnergy, failureCallback));
    }

    private IEnumerator StartHeating(float currEnergy, float totalEnergy, UnityAction failureCallback) {
        heatedSegmentVisual.gameObject.SetActive(true);

        currEnergy -= stateEnergyLostDict[state] * totalEnergy;
        bool isEnergySufficient = currEnergy >= 0f;

        // only heat the entire segment if we have not run out of energy
        float maxScaleValue = isEnergySufficient ? 0.99f : 0.5f;
        float currScaleValue = 0f;
        while (currScaleValue < maxScaleValue) {
            switch (heatedVisualScaleAxis) {
                case HeatedSegmentScaleAxis.X:
                    heatedSegmentVisual.localScale += Vector3.right * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.x;
                    break;
                case HeatedSegmentScaleAxis.Y:
                    heatedSegmentVisual.localScale += Vector3.up * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.y;
                    break;
                case HeatedSegmentScaleAxis.Z:
                    heatedSegmentVisual.localScale += Vector3.forward * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.z;
                    break;
            }

            yield return null;
        }

        IsHeated = true;

        if (!isEnergySufficient) {
            StartCoroutine(StopHeating(failureCallback));
            yield break;
        }

        // we have enough energy to continue heating the adjacent segments
        foreach (ShipEngineConnectorSegment segment in connectedSegments) {
            segment.Heat(currEnergy, totalEnergy, failureCallback);
        }

        if (engine != null) {
            engine.Heat();
        }
    }

    private IEnumerator StopHeating(UnityAction failureCallback) {
        // slow the heating down until it reaches below a certain speed
        float currSpeed = heatedVisualScaleSpeed;
        while (currSpeed > 0.1f) {
            switch (heatedVisualScaleAxis) {
                case HeatedSegmentScaleAxis.X:
                    heatedSegmentVisual.localScale += Vector3.right * currSpeed * Time.deltaTime;
                    break;
                case HeatedSegmentScaleAxis.Y:
                    heatedSegmentVisual.localScale += Vector3.up * currSpeed * Time.deltaTime;
                    break;
                case HeatedSegmentScaleAxis.Z:
                    heatedSegmentVisual.localScale += Vector3.forward * currSpeed * Time.deltaTime;
                    break;
            }

            // decelerate
            currSpeed *= 0.99f;
            yield return null;
        }

        // heating has stopped; wait a moment before starting cooling
        yield return new WaitForSeconds(2f);

        Cool();
        failureCallback();
    }

    public void Cool() {
        if (!IsHeated) {
            return;
        }

        StartCoroutine(StartCooling());
    }

    private IEnumerator StartCooling() {
        float currSpeed = 0.01f;
        float currScaleValue = heatedVisualScaleAxis == HeatedSegmentScaleAxis.X
            ? heatedSegmentVisual.localScale.x
            : heatedVisualScaleAxis == HeatedSegmentScaleAxis.Y
                ? heatedSegmentVisual.localScale.y
                : heatedSegmentVisual.localScale.z;

        while (currScaleValue > 0.01f) {
            switch (heatedVisualScaleAxis) {
                case HeatedSegmentScaleAxis.X:
                    heatedSegmentVisual.localScale -= Vector3.right * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.x;
                    break;
                case HeatedSegmentScaleAxis.Y:
                    heatedSegmentVisual.localScale -= Vector3.up * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.y;
                    break;
                case HeatedSegmentScaleAxis.Z:
                    heatedSegmentVisual.localScale -= Vector3.forward * heatedVisualScaleSpeed * Time.deltaTime;
                    currScaleValue = heatedSegmentVisual.localScale.z;
                    break;
            }

            if (currSpeed < heatedVisualScaleSpeed) {
                currSpeed *= 1.01f;
            }

            yield return null;
        }

        heatedSegmentVisual.gameObject.SetActive(false);
        IsHeated = false;

        foreach (ShipEngineConnectorSegment segment in connectedSegments) {
            segment.Cool();
        }
    }
}

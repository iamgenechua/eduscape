using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipEngineConnectorSegment : MonoBehaviour {

    /// <summary>
    /// The amount of energy conserved after heating a segment of the given state.
    /// </summary>
    private static readonly Dictionary<State, float> stateEnergyConservedDict = new Dictionary<State, float>() {
        [State.BASE] = 0.5f,
        [State.METAL] = 1f,
        [State.WATER] = 0f
    };

    public enum State { BASE, METAL, WATER }

    private State state = State.BASE;

    [SerializeField] private Material heatedMaterial;

    private MeshRenderer mesh;
    private Material currUnheatedMaterial;
    private bool isHeated = false;

    [SerializeField] private ShipEngineConnectorSegment[] connectedSegments;
    [SerializeField] private ShipEngine engine;

    void Awake() {
        mesh = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
        currUnheatedMaterial = mesh.material;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ChangeState(State newState) {
        state = newState;
    }

    public void Heat(float currEnergy, float totalEnergy, UnityAction failureCallback) {
        if (isHeated) {
            return;
        }

        currEnergy -= stateEnergyConservedDict[state] * totalEnergy;

        if (currEnergy <= 0f) {
            isHeated = false;
            failureCallback();
            return;
        }

        isHeated = true;
        mesh.material = heatedMaterial;

        foreach (ShipEngineConnectorSegment segment in connectedSegments) {
            segment.Heat(currEnergy, totalEnergy, failureCallback);
        }

        if (engine != null) {
            engine.Heat();
        }
    }

    public void Cool() {
        if (!isHeated) {
            return;
        }

        isHeated = false;
        mesh.material = currUnheatedMaterial;

        foreach (ShipEngineConnectorSegment segment in connectedSegments) {
            segment.Cool();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineConnector : MonoBehaviour {

    public enum Type { BASE, METAL, WATER }

    private Type type = Type.BASE;

    [SerializeField] private ShipEngine engine;

    [SerializeField] private float timeToHeatSegment;

    [SerializeField] private ShipEngineConnectorSegment[] segments;

    private bool isHeating = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GeneratorHitByElement(Element element) {
        if (!isHeating && element.ElementType == Element.Type.FIRE) {
            StartCoroutine(HeatEngines());
        }
    }

    private bool CanContinueConducting(int numSegmentsHeated) {
        switch (type) {
            case Type.BASE:
                return numSegmentsHeated < segments.Length / 2;
            case Type.METAL:
                return true;
            case Type.WATER:
                return numSegmentsHeated < 1;
            default:
                throw new System.ArgumentException($"Conductivity of {type} not accounted for.");
        }
    }

    private IEnumerator HeatEngines() {
        isHeating = true;
        bool areEnginesHeated = true;

        for (int i = 0; i < segments.Length; i++) {
            yield return new WaitForSeconds(timeToHeatSegment);

            if (CanContinueConducting(i)) {
                segments[i].Heat();
                continue;
            }

            areEnginesHeated = false;
            break;
        }

        if (areEnginesHeated) {
            engine.IsHeated = true;
        } else {
            foreach (ShipEngineConnectorSegment segment in segments) {
                segment.Cool();
            }
        }

        isHeating = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineConnector : MonoBehaviour {

    [SerializeField] private ShipEngine engine;
    [SerializeField] private ShipEngineConnectorSegment firstSegment;

    private bool isHeating = false;

    // Start is called before the first frame update
    void Start() {
        Invoke(nameof(StartHeating), 3f);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GeneratorHitByElement(Element element) {
        if (!isHeating && element.ElementType == Element.Type.FIRE) {
            StartHeating();
        }
    }

    private void StartHeating() {
        isHeating = true;
        float inputEnergy = 100f;
        firstSegment.Heat(inputEnergy, inputEnergy,
            () => StartCoroutine(
                GameManager.Instance.WaitForConditionBeforeAction(
                    () => !firstSegment.IsHeated,
                    () => isHeating = false)));
    }
}

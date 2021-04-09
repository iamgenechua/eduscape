using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineConnectorSegment : MonoBehaviour {

    private bool isHeated = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Heat() {
        if (isHeated) {
            return;
        }

        isHeated = true;
    }

    public void Cool() {
        if (!isHeated) {
            return;
        }

        isHeated = false;
    }
}

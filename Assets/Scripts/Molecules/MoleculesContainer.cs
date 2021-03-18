using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainer : MonoBehaviour {

    [SerializeField] private float roomTemperature = 36.0f;
    [SerializeField] private float raiseTempAmount = 5.0f;
    private float temperature;

    [SerializeField] private Molecule[] molecules;

    // Start is called before the first frame update
    void Start() {
        temperature = roomTemperature;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void RaiseTemperature() {
        float newTemp = temperature + raiseTempAmount;
        foreach (Molecule mol in molecules) {
            mol.ChangeSpeed(temperature, newTemp);
        }

        temperature = newTemp;
    }
}

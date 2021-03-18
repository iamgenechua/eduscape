using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainer : MonoBehaviour {

    [SerializeField] private float roomTemperature = 36.0f;
    [SerializeField] private float raiseTempAmount = 5.0f;
    [SerializeField] private float maxTemperature = 65f;
    private float temperature;

    [SerializeField] private Molecule[] molecules;

    private bool isExploding = false;

    // Start is called before the first frame update
    void Start() {
        temperature = roomTemperature;
        StartCoroutine(RaiseTemp());
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator RaiseTemp() {
        while (true) {
            yield return new WaitForSeconds(3);
            RaiseTemperature();
        }
    }

    public void RaiseTemperature() {
        if (isExploding) {
            return;
        }

        float newTemp = temperature + raiseTempAmount;
        foreach (Molecule mol in molecules) {
            mol.ChangeSpeed(temperature, newTemp);
        }

        temperature = newTemp;
        if (temperature > maxTemperature) {
            StartExplosion();
        }
    }

    private void StartExplosion() {
        isExploding = true;
        Invoke(nameof(Explode), 3f);
    }

    private void Explode() {
        foreach (Molecule mol in molecules) {
            Destroy(mol.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Molecule>()) {
            // molecule has broken free of the container
            Explode();
        }
    }
}

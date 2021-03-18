using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainer : MonoBehaviour {

    [Header("Temperature")]

    [SerializeField] private TemperatureDisplay tempDisplay;
    [SerializeField] private float roomTemperature = 36.0f;
    [SerializeField] private float raiseTempAmount = 5.0f;
    [SerializeField] private float maxTemperature = 65f;
    private float temperature;

    [Header("Molecules")]

    [SerializeField] private Molecule[] molecules;

    [Header("Explosion")]

    [SerializeField] private GameObject explosionPrefab;
    private bool isExploding = false;

    // Start is called before the first frame update
    void Start() {
        temperature = roomTemperature;
        tempDisplay.UpdateDisplay(temperature, temperature > maxTemperature);
        StartCoroutine(RaiseTemp());
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

        tempDisplay.UpdateDisplay(temperature, temperature > maxTemperature);
    }

    private void StartExplosion() {
        isExploding = true;
        Invoke(nameof(Explode), 3f);
    }

    private void Explode() {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        foreach (Molecule mol in molecules) {
            Destroy(mol.gameObject);
        }

        Destroy(tempDisplay.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Molecule>()) {
            // molecule has broken free of the container
            Explode();
        }
    }
}

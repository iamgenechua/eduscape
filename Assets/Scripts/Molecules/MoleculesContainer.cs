using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainer : MonoBehaviour {

    [SerializeField] private TemperatureDisplay tempDisplay;
    [SerializeField] private float roomTemperature = 36.0f;
    [SerializeField] private float raiseTempAmount = 5.0f;
    [SerializeField] private float maxTemperature = 65f;
    private float temperature;

    [Space(10)]

    [SerializeField] private Molecule[] molecules;

    [Space(10)]

    [SerializeField] private GameObject explosionPrefab;
    private bool isExploding = false;

    [Space(10)]

    [SerializeField] private RationaleCanvas rationaleCanvas;

    // Start is called before the first frame update
    void Start() {
        temperature = roomTemperature;
        tempDisplay.UpdateDisplay(temperature, temperature > maxTemperature);
        // StartCoroutine(RaiseTempOverTime());
    }

    private IEnumerator RaiseTempOverTime() {
        while (true) {
            yield return new WaitForSeconds(1);
            RaiseTemperature();
        }
    }

    public void HitByElementProjectile(Element element) {
        if (element.ElementType == Element.Type.FIRE) {
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

        rationaleCanvas.FadeIn();

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

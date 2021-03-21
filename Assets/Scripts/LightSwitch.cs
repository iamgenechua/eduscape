using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    [SerializeField] private Light[] lights;

    private void Flip() {
        foreach (Light light in lights) {
            light.gameObject.SetActive(!light.gameObject.activeInHierarchy);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Hand")) {
            Flip();
        }
    }
}

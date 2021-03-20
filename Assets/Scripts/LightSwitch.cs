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

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Hand")) {
            Flip();
        }
    }

}

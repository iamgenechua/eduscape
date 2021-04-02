using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiLight : MonoBehaviour {

    private MeshRenderer mesh;
    [SerializeField] private int lightMaterialIndex = 1;
    [SerializeField] private Material unlitMaterial;
    [SerializeField] private Material litMaterial;

    private Light pointLight;

    void Awake() {
        mesh = GetComponent<MeshRenderer>();
        pointLight = GetComponentInChildren<Light>();
    }

    public void TurnOn() {
        Material[] materials = mesh.materials;
        materials[lightMaterialIndex] = litMaterial;
        mesh.materials = materials;
        pointLight.gameObject.SetActive(true);
    }

    public void TurnOff() {
        Material[] materials = mesh.materials;
        materials[lightMaterialIndex] = unlitMaterial;
        mesh.materials = materials;
        pointLight.gameObject.SetActive(false);
    }
}

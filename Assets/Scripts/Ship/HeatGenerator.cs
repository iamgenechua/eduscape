using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatGenerator : MonoBehaviour {

    private MeshRenderer generatorMesh;
    private AudioSource audioSource;

    [Header("Visuals")]

    [SerializeField] private GameObject fire;
    [SerializeField] private int generatorMeshMaterialIndex;
    [SerializeField] private Material generatorOffMaterial;
    [SerializeField] private Material generatorLitMaterial;

    [Header("Audio")]

    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip stopSound;

    [Header("Logic")]

    [SerializeField] private ShipEngineConnectorSegment firstSegment;
    [SerializeField] private ConnectorStateChanger stateChanger;

    private bool isHeating = false;

    void Awake() {
        generatorMesh = GetComponent<MeshRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorOffMaterial;
        generatorMesh.materials = generatorMaterials;

        fire.SetActive(false);
    }

    public void GeneratorHitByElement(Element element) {
        if (!isHeating && !stateChanger.IsChanging && element.ElementType == Element.Type.FIRE) {
            StartHeating();
        }
    }

    public void StartHeating() {
        isHeating = true;

        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorLitMaterial;
        generatorMesh.materials = generatorMaterials;

        audioSource.PlayOneShot(startSound);
        audioSource.Play();

        fire.SetActive(true);

        float inputEnergy = 100f;
        firstSegment.Heat(inputEnergy, inputEnergy,
            () => StartCoroutine(GameManager.Instance.WaitForConditionBeforeAction(
                () => !firstSegment.IsHeated, StopHeating)));
    }

    private void StopHeating() {
        isHeating = false;

        audioSource.Stop();
        audioSource.PlayOneShot(stopSound);

        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorOffMaterial;
        generatorMesh.materials = generatorMaterials;

        fire.SetActive(false);
    }
}

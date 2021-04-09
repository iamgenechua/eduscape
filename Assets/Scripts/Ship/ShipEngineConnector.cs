using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineConnector : MonoBehaviour {

    [SerializeField] private MeshRenderer generatorMesh;
    [SerializeField] private int generatorMeshMaterialIndex;
    [SerializeField] private Material generatorOffMaterial;
    [SerializeField] private Material generatorLitMaterial;

    [SerializeField] private GameObject fire;
    [SerializeField] private ShipEngineConnectorSegment firstSegment;

    private bool isHeating = false;

    // Start is called before the first frame update
    void Start() {
        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorOffMaterial;
        generatorMesh.materials = generatorMaterials;

        fire.SetActive(false);
        Invoke(nameof(StartHeating), 7f);
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

        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorLitMaterial;
        generatorMesh.materials = generatorMaterials;

        fire.SetActive(true);

        float inputEnergy = 100f;
        firstSegment.Heat(inputEnergy, inputEnergy,
            () => StartCoroutine(GameManager.Instance.WaitForConditionBeforeAction(
                () => !firstSegment.IsHeated, StopHeating)));
    }

    private void StopHeating() {
        isHeating = false;

        Material[] generatorMaterials = generatorMesh.materials;
        generatorMaterials[generatorMeshMaterialIndex] = generatorOffMaterial;
        generatorMesh.materials = generatorMaterials;

        fire.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MonoBehaviour {

    [SerializeField] private MeshRenderer heatMesh;

    [SerializeField] private Material cooledMaterial;
    [SerializeField] private Material heatedMaterial;

    public bool IsHeated { get; private set; }

    // Start is called before the first frame update
    void Start() {
        Cool();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Heat() {
        IsHeated = true;
        heatMesh.material = heatedMaterial;
    }

    public void Cool() {
        IsHeated = false;
        heatMesh.material = cooledMaterial;
    }
}

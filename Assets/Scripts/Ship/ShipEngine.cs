using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField] private MeshRenderer heatMesh;

    [SerializeField] private Material cooledMaterial;
    [SerializeField] private Material heatedMaterial;

    [SerializeField] private AudioClip heatSound;

    public bool IsHeated { get; private set; }

    private void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

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

        audioSource.Play();
        audioSource.PlayOneShot(heatSound);
    }

    public void Cool() {
        IsHeated = false;
        heatMesh.material = cooledMaterial;

        audioSource.Stop();
        audioSource.PlayOneShot(heatSound);
    }
}

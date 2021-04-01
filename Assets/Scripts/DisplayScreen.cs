using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreen : MonoBehaviour {

    [SerializeField] private MeshRenderer screenMesh;
    [SerializeField] private Material screenMaterial;

    [SerializeField] private Canvas displayCanvas;

    private Animator anim;
    [SerializeField] private string stowAnimParam;
    public bool IsStowed { get => anim.GetBool(stowAnimParam); }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        Stow();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Stow() {
        anim.SetBool(stowAnimParam, true);
    }

    public void Unstow() {
        anim.SetBool(stowAnimParam, false);
    }

    public void ActivateScreen() {
        screenMesh.material = screenMaterial;
        displayCanvas.gameObject.SetActive(true);
    }

    public void DeactivateScreen() {
        displayCanvas.gameObject.SetActive(false);
        screenMesh.material = null;
    }
}

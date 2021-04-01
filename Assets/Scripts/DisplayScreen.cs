using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreen : MonoBehaviour {

    private Animator anim;

    [SerializeField] private MeshRenderer screenMesh;
    [SerializeField] private Material screenMaterial;

    [SerializeField] private Canvas displayCanvas;

    [SerializeField] private string stowAnimParam;
    [SerializeField] private bool isStowedAtStart = true;

    public bool IsStowed { get => anim.GetBool(stowAnimParam); }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        if (isStowedAtStart) {
            Stow();
        } else {
            Unstow();
        }
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

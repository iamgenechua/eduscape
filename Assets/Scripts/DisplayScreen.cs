using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScreen : MonoBehaviour {

    private Animator anim;
    private TextMeshProUGUI text;

    [SerializeField] private MeshRenderer screenMesh;
    [SerializeField] private Material screenMaterial;
    [SerializeField] private Material dangerMaterial;

    [SerializeField] private Canvas displayCanvas;

    [SerializeField] private string stowAnimParam;
    [SerializeField] private bool isStowedAtStart = true;

    public bool IsStowed { get => anim.GetBool(stowAnimParam); }

    void Awake() {
        anim = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start() {
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

    public void ActivateDangerMode() {
        screenMesh.material = dangerMaterial;
        displayCanvas.gameObject.SetActive(true);
    }

    public void DeactivateScreen() {
        displayCanvas.gameObject.SetActive(false);
        screenMesh.material = null;
    }

    public void SetText(string textToSet) {
        text.text = textToSet;
    }
}

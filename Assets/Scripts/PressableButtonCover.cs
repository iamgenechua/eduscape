using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButtonCover : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string openCloseAnimParam = "isOpen";

    public bool IsOpen { get => anim.GetBool(openCloseAnimParam); }

    [SerializeField] private GameObject dummyButton;
    [SerializeField] private GameObject actualButton;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        SetDummyActualButtons();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Open() {
        anim.SetBool(openCloseAnimParam, true);
        SetDummyActualButtons();
    }

    public void Close() {
        anim.SetBool(openCloseAnimParam, false);
        SetDummyActualButtons();
    }

    private void SetDummyActualButtons() {
        dummyButton.SetActive(!IsOpen);
        actualButton.SetActive(IsOpen);
    }
}

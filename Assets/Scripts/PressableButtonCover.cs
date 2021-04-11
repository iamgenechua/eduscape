using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButtonCover : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string openCloseAnimParam = "isOpen";

    void Awake() {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Open() {
        anim.SetBool(openCloseAnimParam, true);
    }

    public void Close() {
        anim.SetBool(openCloseAnimParam, false);
    }
}

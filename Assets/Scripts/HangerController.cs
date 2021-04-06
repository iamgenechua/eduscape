using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerController : MonoBehaviour {

    [SerializeField] private Animator[] wallAnims;
    [SerializeField] private string wallAnimParam = "isOpen";

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OpenWalls() {
        foreach (Animator wallAnim in wallAnims) {
            wallAnim.SetBool(wallAnimParam, true);
        }
    }

    public void CloseWalls() {
        foreach (Animator wallAnim in wallAnims) {
            wallAnim.SetBool(wallAnimParam, false);
        }
    }
}

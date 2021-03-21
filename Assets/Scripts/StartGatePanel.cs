using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGatePanel : MonoBehaviour {

    private Animator anim;

    [SerializeField] private Element firePickup;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Lower() {
        anim.SetTrigger("Lower");
        firePickup.gameObject.SetActive(true);
    }
}

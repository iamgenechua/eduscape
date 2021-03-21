using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGatePanel : MonoBehaviour {

    private Animator anim;

    [Tooltip("Object containing start area light objects.")]
    [SerializeField] private GameObject startAreaLights;

    [SerializeField] private Element firePickup;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Lower() {
        anim.SetTrigger("Lower");
        firePickup.gameObject.SetActive(true);
    }

    public void ShortCircuit() {
        startAreaLights.SetActive(false);
        enabled = false;
    }

    public void HitByElement(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            Lower();
            ShortCircuit();
        }
    }
}

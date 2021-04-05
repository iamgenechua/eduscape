using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckDoorPanel : MonoBehaviour {

    private Rigidbody rb;
    private ElementTarget elementTarget;

    [SerializeField] private FadeCanvas rationaleCanvas;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        elementTarget = GetComponent<ElementTarget>();
    }

    public void HitByElement(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            rb.isKinematic = false;
            elementTarget.enabled = false;
            rationaleCanvas.FadeIn();
        }
    }
}

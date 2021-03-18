using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainerWalls : MonoBehaviour {

    private MoleculesContainer container;

    // Start is called before the first frame update
    void Start() {
        container = GetComponentInParent<MoleculesContainer>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.GetComponent<Element>()?.ElementType == Element.Type.FIRE) {
            container.RaiseTemperature();
        }
    }
}

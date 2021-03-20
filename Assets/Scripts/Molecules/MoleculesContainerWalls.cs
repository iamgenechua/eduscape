using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesContainerWalls : MonoBehaviour {

    private MoleculesContainer container;

    // Start is called before the first frame update
    void Start() {
        container = GetComponentInParent<MoleculesContainer>();
    }

    public void RaiseTempWhenHitWithFire(Element elementHit) {
        if (elementHit.ElementType == Element.Type.FIRE) {
            container.RaiseTemperature();
        }
    }
}

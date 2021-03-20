using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTarget : MonoBehaviour {

    [SerializeField] private ElementEvent hitByElementProjectile;

    public void GetHitByElementProjectile(Element element) {
        hitByElementProjectile.Invoke(element);
    }
}

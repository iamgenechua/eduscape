using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTarget : MonoBehaviour {

    [SerializeField] private ElementEvent contactedByElementEvent;

    public void GetContactedByElement(Element element) {
        contactedByElementEvent.Invoke(element);
    }
}

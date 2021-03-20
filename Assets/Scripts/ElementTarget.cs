using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ElementEvent : UnityEvent<Element.Type> {}

public class ElementTarget : MonoBehaviour {

    [SerializeField] private ElementEvent contactedByElement;

    public void GetContactedByElement(Element.Type elementType) {
        contactedByElement.Invoke(elementType);
    }
}

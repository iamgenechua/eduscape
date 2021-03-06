using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ElementEvent : UnityEvent<Element> { }

public class Element : MonoBehaviour {

    public enum Type { FIRE, METAL, WATER, AIR }
    [SerializeField] protected Type type;
    public Type ElementType { get => type; protected set => type = value; }

    protected virtual void Awake() {}

    protected virtual void Start() {}

    protected virtual void OnEnable() {}

    public static bool operator== (Element e1, Element e2) {
        bool isE1Null = e1 is null;
        bool isE2Null = e2 is null;
        if (isE1Null && isE2Null) {
            return true;
        } else if ((isE1Null && !isE2Null) || (!isE1Null && isE2Null)) {
            return false;
        }

        return e1.ElementType == e2.ElementType;
    }

    public static bool operator!= (Element e1, Element e2) {
        return !(e1 == e2);
    }

    public override bool Equals(object other) {
        Element otherElement = other as Element;
        if (otherElement == null) {
            return false;
        }

        return this == otherElement;
    }

    public override int GetHashCode() {
        return ElementType.GetHashCode();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {

    public enum Type { FIRE, METAL, WATER, AIR }

    [SerializeField] private Type type;

    public Type ElementType { get => type; }
}

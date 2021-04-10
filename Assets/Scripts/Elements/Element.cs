using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ElementEvent : UnityEvent<Element> { }

public class Element : MonoBehaviour {

    public enum Type { FIRE, METAL, WATER, AIR }
    [SerializeField] private Type type;
    public Type ElementType { get => type; private set => type = value; }

    public enum State { PICKUP, HELD, PROJECTILE }
    [SerializeField] private State state;
    public State ElementState { get => state; private set => state = value; }

    [Space(10)]

    [Tooltip("The held element corresponding to this pickup element.")]
    [SerializeField] private Element pickupCorrespondingHeld;
    [SerializeField] private ElementEvent elementPickedUpEvent;

    [Space(10)]

    [SerializeField] private Element projectilePrefab;

    private AudioSource restingAudioSource;

    void Awake() {
        restingAudioSource = GetComponentInChildren<AudioSource>();
    }

    void OnEnable() {
        if (ElementState == State.PICKUP || ElementState == State.HELD) {
            restingAudioSource.Play();
        }
    }

    public void PickUp() {
        elementPickedUpEvent.Invoke(pickupCorrespondingHeld);
        Destroy(gameObject);
    }

    public void Shoot(Vector3 direction, float force) {
        Element projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * force;
    }

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

    private void OnCollisionEnter(Collision collision) {
        if (ElementState == State.PROJECTILE && !collision.gameObject.CompareTag("Hand") && !collision.gameObject.CompareTag("Player")) {
            ElementTarget elementTarget = collision.gameObject.GetComponent<ElementTarget>();
            if (elementTarget != null) {
                elementTarget.GetHitByElementProjectile(this);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (ElementState == State.PICKUP && other.CompareTag("Hand")) {
            PickUp();
        }
    }
}

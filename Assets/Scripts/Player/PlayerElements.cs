using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerElements : MonoBehaviour {

    private List<Element> elements;
    public AudioSource firePickup;
    public AudioSource fireHold;
    public AudioSource fireShoot;
    public AudioSource fireSwitch;

    public AudioSource waterSwitch;
    public AudioSource waterHold;
    public AudioSource waterShoot;

    public AudioSource steelSwitch;
    public AudioSource steelHold;
    public AudioSource steelShoot;
    
    [Tooltip("Held elements added here will be given to the player at game start.")]
    [SerializeField] private Element[] startingElements;

    [SerializeField] private UnityEvent switchToElementEvent;
    [SerializeField] private UnityEvent switchFromElementsEvent;

    private int activeElementIndex = -1;
    public Element ActiveElement {
        get => activeElementIndex == -1 ? null : elements[activeElementIndex];
        set {
            if (ActiveElement != null) {
                ActiveElement.gameObject.SetActive(false);
            }

            activeElementIndex = value == null ? -1 : elements.IndexOf(value);

            if (ActiveElement != null) {
                ActiveElement.gameObject.SetActive(true);
                switchToElementEvent.Invoke();
            } else {
                switchFromElementsEvent.Invoke();
            }
        }
    }

    [SerializeField] private float shootForce;

    // Start is called before the first frame update
    void Start() {
        elements = new List<Element>(startingElements);
        if (ActiveElement) {
          switch (ActiveElement.ElementType.ToString()) {
                case "WATER":
                    waterHold.Play();
                    break;
                case "FIRE":
                    fireHold.Play();
                    break;
                case "METAL":
                    steelHold.Play();
                    break;
            }  
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddElement(Element element) {
        elements.Add(element);
        ActiveElement = element;

        switch (ActiveElement.ElementType.ToString()) {
            case "WATER":
                waterSwitch.Play();
                break;
            case "FIRE":
                firePickup.Play();
                break;
            case "METAL":
                steelSwitch.Play();
                break;
        }
    }

    public void RemoveElement(Element element) {
        if (ActiveElement == element) {
            ActiveElement = null;
        }
        elements.Remove(element);
    }

    public void CycleActiveElement() {
        ActiveElement = activeElementIndex == elements.Count - 1
            ? null
            : elements[activeElementIndex + 1];

        waterHold.Stop();
        fireHold.Stop();
        steelHold.Stop();

        switch (ActiveElement.ElementType.ToString()) {
            case "WATER":
                waterSwitch.Play();
                waterHold.Play();
                break;
            case "FIRE":
                fireSwitch.Play();
                fireHold.Play();
                break;
            case "METAL":
                steelSwitch.Play();
                steelHold.Play();
                break;
        }
    }

    public IEnumerator ShootActiveElement() {
        Element shotElement = ActiveElement;
        ActiveElement.gameObject.SetActive(false);
        ActiveElement.Shoot(transform.forward, shootForce);

        yield return new WaitForSeconds(0.5f);
        
        if (ActiveElement == shotElement) {
            ActiveElement.gameObject.SetActive(true);
        }

        switch (ActiveElement.ElementType.ToString()) {
            case "WATER":
                waterShoot.Play();
                break;
            case "FIRE":
                fireShoot.Play();
                break;
            case "METAL":
                steelShoot.Play();
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerElements : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// The elements the player currently has.
    /// </summary>
    private List<Element> elements;

    [Tooltip("Held elements added here will be given to the player at game start.")]
    [SerializeField] private Element[] startingElements;

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

    [Space(10)]

    [SerializeField] private AudioClip metalSwitchSound;
    [SerializeField] private AudioClip waterSwitchSound;
    [SerializeField] private AudioClip fireSwitchSound;

    [Space(10)]

    [SerializeField] private UnityEvent switchToElementEvent;
    public UnityEvent SwitchToElementEvent { get => switchToElementEvent; }

    [Space(10)]

    [SerializeField] private UnityEvent switchFromElementsEvent;
    public UnityEvent SwitchFromElementEvent { get => switchFromElementsEvent; }

    [Space(10)]

    [SerializeField] private float shootForce;

    [SerializeField] private AudioClip metalShootSound;
    [SerializeField] private AudioClip waterShootSound;
    [SerializeField] private AudioClip fireShootSound;

    [Space(10)]

    [SerializeField] private UnityEvent shootElementEvent;
    public UnityEvent ShootElementEvent { get => shootElementEvent; }

    void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        elements = new List<Element>(startingElements);
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddElement(Element element) {
        elements.Add(element);
        ActiveElement = element;
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

        if (ActiveElement == null) {
            return;
        }

        switch (ActiveElement.ElementType) {
            case Element.Type.METAL:
                audioSource.PlayOneShot(metalSwitchSound);
                break;
            case Element.Type.WATER:
                audioSource.PlayOneShot(waterSwitchSound);
                break;
            case Element.Type.FIRE:
                audioSource.PlayOneShot(fireSwitchSound);
                break;
            default:
                throw new System.ArgumentException($"Active element of type {ActiveElement.ElementType} has no switch sound.");
        }
    }

    public IEnumerator ShootActiveElement() {
        Element shotElement = ActiveElement;
        ActiveElement.gameObject.SetActive(false);

        switch (shotElement.ElementType) {
            case Element.Type.METAL:
                audioSource.PlayOneShot(metalShootSound);
                break;
            case Element.Type.WATER:
                audioSource.PlayOneShot(waterShootSound);
                break;
            case Element.Type.FIRE:
                audioSource.PlayOneShot(fireShootSound);
                break;
            default:
                Debug.LogWarning($"{shotElement.ElementType} does not have corresponding shoot sound.");
                break;
        }

        ActiveElement.Shoot(transform.forward, shootForce);
        shootElementEvent.Invoke();

        yield return new WaitForSeconds(0.5f);
        
        if (ActiveElement == shotElement) {
            ActiveElement.gameObject.SetActive(true);
        }
    }

    private void OnDestroy() {
        switchToElementEvent.RemoveAllListeners();
        switchFromElementsEvent.RemoveAllListeners();
        shootElementEvent.RemoveAllListeners();
    }
}

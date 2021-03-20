using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElements : MonoBehaviour {

    private List<Element> elements;

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
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        elements = new List<Element>();
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
    }
}

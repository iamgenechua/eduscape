using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RationaleSummary : MonoBehaviour {

    [SerializeField] private string[] rationaleTexts;
    private int currTextIndex;

    private TextMeshProUGUI text;

    void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start() {
        currTextIndex = 0;
        text.text = rationaleTexts[currTextIndex];
    }

    // Update is called once per frame
    void Update() {

    }

    public void DisplayPrevious() {
        currTextIndex = currTextIndex == 0 ? rationaleTexts.Length - 1 : currTextIndex - 1;
        text.text = rationaleTexts[currTextIndex];
    }

    public void DisplayNext() {
        currTextIndex = currTextIndex == rationaleTexts.Length - 1 ? 0 : currTextIndex + 1;
        text.text = rationaleTexts[currTextIndex];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScreen : MonoBehaviour {

    private Animator anim;
    private TextMeshProUGUI text;

    [SerializeField] private MeshRenderer screenMesh;
    [SerializeField] private Material screenMaterial;
    [SerializeField] private Material dangerMaterial;
    [SerializeField] private float warningDuration = 5f;
    [SerializeField] private float warningPulseDuration = 0.75f;

    public bool IsPulsingWarningScreen { get; private set; }

    [SerializeField] private Canvas displayCanvas;
    [SerializeField] private float timeBetweenCharacters = 0.05f;
    [SerializeField] private float punctuationPauseTime = 0.5f;

    public bool IsRollingOut { get; private set; }

    [SerializeField] private string stowAnimParam;
    [SerializeField] private bool isStowedAtStart = true;

    public bool IsStowed { get => anim.GetBool(stowAnimParam); }

    void Awake() {
        anim = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start() {
        IsPulsingWarningScreen = false;
        IsRollingOut = false;
        if (isStowedAtStart) {
            Stow();
        } else {
            Unstow();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Stow() {
        anim.SetBool(stowAnimParam, true);
    }

    public void Unstow() {
        anim.SetBool(stowAnimParam, false);
    }

    public void ActivateScreen() {
        screenMesh.material = screenMaterial;
        displayCanvas.gameObject.SetActive(true);
    }

    public void DisplayWarning() {
        StartCoroutine(PulseWarningScreen());
    }

    private IEnumerator PulseWarningScreen() {
        IsPulsingWarningScreen = true;

        text.text = "WARNING";

        bool isRed = false;
        int numPulses = Mathf.FloorToInt(warningDuration / warningPulseDuration);
        if (numPulses % 2 != 0) {
            numPulses -= 1;
        }

        for (int i = 0; i < numPulses; i++) {
            screenMesh.material = isRed ? null : dangerMaterial;
            displayCanvas.gameObject.SetActive(!isRed);
            isRed = !isRed;
            yield return new WaitForSeconds(warningPulseDuration);
        }

        screenMesh.material = dangerMaterial;
        displayCanvas.gameObject.SetActive(true);
        IsPulsingWarningScreen = false;
    }

    public void DeactivateScreen() {
        displayCanvas.gameObject.SetActive(false);
        screenMesh.material = null;
    }

    public void SetText(string textToSet, bool rollOut = true) {
        if (rollOut) {
            StartCoroutine(RollOutText(textToSet));
        } else {
            text.text = textToSet;
        }
    }

    private IEnumerator RollOutText(string textToSet) {
        yield return new WaitUntil(() => !IsRollingOut);

        IsRollingOut = true;
        text.text = "";

        string currText = "";
        for (int i = 0; i < textToSet.Length; i++) {
            currText += textToSet[i];
            text.text = currText;
            if (i < textToSet.Length - 1) {
                yield return new WaitForSeconds(IsPunctuationPause(textToSet[i])
                    ? punctuationPauseTime
                    : timeBetweenCharacters);
            }
        }

        IsRollingOut = false;
    }

    private bool IsPunctuationPause(char character) {
        return character == '.' || character == ','
            || character == '?' || character == ';'
            || character == ':' || character == '-';
    }
}

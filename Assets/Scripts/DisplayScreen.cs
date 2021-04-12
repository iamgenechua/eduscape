using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreen : MonoBehaviour {

    private Animator anim;
    private TextRollout text;

    public bool IsRollingOut { get => text.IsRollingOut; }

    [Header("Screen")]

    [SerializeField] private MeshRenderer screenMesh;
    [SerializeField] private Material screenMaterial;
    [SerializeField] private Material deactivatedMaterial;
    [SerializeField] private Material dangerMaterial;
    [SerializeField] private float warningDuration;
    [SerializeField] private float warningPulseDuration;

    [Space(10)]

    [SerializeField] private AudioSource screenSoundSource;
    [SerializeField] private AudioClip activateSound;
    [SerializeField] private AudioClip deactivateSound;

    public bool IsPulsingWarningScreen { get; private set; }

    [Header("Stowing")]

    [SerializeField] private bool isStowedAtStart = true;
    [SerializeField] private string stowAnimParam;

    [Space(10)]

    [SerializeField] private AudioSource flipSoundSource;
    [SerializeField] private AudioClip flipWhooshSound;
    [SerializeField] private AudioClip flipEndSound;

    public bool IsStowed {
        get {
            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            return animStateInfo.IsName("Stowed") || animStateInfo.IsName("Unstow");
        }
    }

    void Awake() {
        anim = GetComponent<Animator>();
        text = GetComponentInChildren<TextRollout>();
    }

    // Start is called before the first frame update
    void Start() {
        IsPulsingWarningScreen = false;
        if (isStowedAtStart) {
            Stow();
        } else {
            Unstow();
        }
    }

    public void Stow() {
        anim.SetBool(stowAnimParam, true);
    }

    public void Unstow() {
        anim.SetBool(stowAnimParam, false);
    }

    public void PlayFlipWhooshSound() {
        flipSoundSource.PlayOneShot(flipWhooshSound);
    }

    public void PlayFlipEndSound() {
        flipSoundSource.PlayOneShot(flipEndSound);
    }

    public void ActivateScreen() {
        screenMesh.material = screenMaterial;
        text.gameObject.SetActive(true);
        screenSoundSource.PlayOneShot(activateSound);
    }

    public void SetText(string textToSet, bool rollOut = true) {
        if (rollOut) {
            text.StartRollOut(textToSet);
        } else {
            text.Text = textToSet;
        }
    }

    public void DisplayWarning() {
        StartCoroutine(PulseWarningScreen());
    }

    private IEnumerator PulseWarningScreen() {
        IsPulsingWarningScreen = true;

        text.Text = "WARNING";

        bool isRed = false;
        int numPulses = Mathf.FloorToInt(warningDuration / warningPulseDuration);
        if (numPulses % 2 != 0) {
            numPulses -= 1;
        }

        for (int i = 0; i < numPulses; i++) {
            screenMesh.material = isRed ? null : dangerMaterial;
            text.gameObject.SetActive(!isRed);
            isRed = !isRed;
            yield return new WaitForSeconds(warningPulseDuration);
        }

        screenMesh.material = dangerMaterial;
        text.gameObject.SetActive(true);
        IsPulsingWarningScreen = false;
    }

    public void DeactivateScreen() {
        text.gameObject.SetActive(false);
        screenMesh.material = deactivatedMaterial;
        screenSoundSource.PlayOneShot(deactivateSound);
    }
}

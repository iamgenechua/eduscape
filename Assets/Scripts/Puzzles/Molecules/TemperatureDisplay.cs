using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureDisplay : MonoBehaviour {

    private TextMeshPro text;
    private AudioSource audioSource;

    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private TMP_FontAsset dangerFontAsset;

    [SerializeField] private AudioClip tempIncreaseClip;
    [SerializeField] private AudioClip maxTempClip;
    [SerializeField] private float maxTempSoundLoopCount;

    void Awake() {
        text = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start() {
        audioSource.clip = tempIncreaseClip;
    }

    public void UpdateDisplay(float newTemp, bool isTempAboveMax) {
        text.text = newTemp.ToString("0.0") + " °C";

        if (isTempAboveMax) {
            SwitchFontAsset(dangerFontAsset);
            StartCoroutine(PlayMaxTempSound());
        } else {
            audioSource.Play();
        }
    }

    private void SwitchFontAsset(TMP_FontAsset asset) {
        text.font = asset;
    }

    private IEnumerator PlayMaxTempSound() {
        audioSource.clip = maxTempClip;

        for (int i = 0; i < maxTempSoundLoopCount; i++) {
            audioSource.Play();
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
    }
}

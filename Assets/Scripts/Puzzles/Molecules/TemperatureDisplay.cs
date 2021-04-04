using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureDisplay : MonoBehaviour {

    public AudioSource tempIncreaseAudioSource;
    public AudioSource tempMaxAudioSource;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private TMP_FontAsset dangerFontAsset;

    public void UpdateDisplay(float newTemp, bool isTempAboveMax) {
        tempIncreaseAudioSource.Play();
        text.text = newTemp.ToString("0.0") + " �C";
        if (isTempAboveMax) {
            tempMaxAudioSource.Play();
            SwitchFontAsset(dangerFontAsset);
        }
    }

    private void SwitchFontAsset(TMP_FontAsset asset) {
        text.font = asset;
    }
}

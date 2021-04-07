using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureDisplay : MonoBehaviour {

    private TextMeshPro text;

    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private TMP_FontAsset dangerFontAsset;

    void Awake() {
        text = GetComponent<TextMeshPro>();
    }

    public void UpdateDisplay(float newTemp, bool isTempAboveMax) {
        text.text = newTemp.ToString("0.0") + " °C";
        if (isTempAboveMax) {
            SwitchFontAsset(dangerFontAsset);
        }
    }

    private void SwitchFontAsset(TMP_FontAsset asset) {
        text.font = asset;
    }
}

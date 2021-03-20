using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureDisplay : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private TMP_FontAsset dangerFontAsset;

    public void UpdateDisplay(float newTemp, bool isTempAboveMax) {
        text.text = newTemp.ToString("0.0");
        if (isTempAboveMax) {
            SwitchFontAsset(dangerFontAsset);
        }
    }

    private void SwitchFontAsset(TMP_FontAsset asset) {
        text.font = asset;
    }
}

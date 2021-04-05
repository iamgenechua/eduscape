using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour {

    [SerializeField] private Level[] levels;
    private int selectedLevelIndex = 0;

    public Level SelectedLevel { get => levels[selectedLevelIndex]; }

    [SerializeField] private TextMeshProUGUI selectedLevelScreenText;

    // Start is called before the first frame update
    void Start() {
        UpdateScreenText();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void LoadSelectedLevel() {
        if (SelectedLevel.IsAvailable) {
            GameManager.Instance.LoadLevel(SelectedLevel);
        }
    }

    public void SelectNextLevel() {
        selectedLevelIndex = (selectedLevelIndex + 1) % levels.Length;
        UpdateScreenText();
    }

    public void SelectPreviousLevel() {
        selectedLevelIndex--;
        if (selectedLevelIndex < 0) {
            selectedLevelIndex = levels.Length - 1;
        }

        UpdateScreenText();
    }

    private void UpdateScreenText() {
        selectedLevelScreenText.text = SelectedLevel.LevelName;
    }
}

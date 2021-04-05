using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

    [SerializeField] private Level[] levels;
    private int selectedLevelIndex = 0;

    public Level SelectedLevel { get => levels[selectedLevelIndex]; }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void LoadSelectedLevel() {
        GameManager.Instance.LoadLevel(SelectedLevel);
    }

    public void SelectNextLevel() {
        selectedLevelIndex = (selectedLevelIndex + 1) % levels.Length;
    }

    public void SelectPreviousLevel() {
        selectedLevelIndex--;
        if (selectedLevelIndex < 0) {
            selectedLevelIndex = levels.Length - 1;
        }
    }
}

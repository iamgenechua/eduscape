using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [SerializeField] private FadePlayerView fade;

    void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }

    public void QuitGame() {
        if (fade.IsFading) {
            return;
        }

        fade.FadeOutCompleteEvent.AddListener(() => {
            if (Application.isEditor) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else {
                Application.Quit();
            }
        });

        fade.FadeOut();
    }
}

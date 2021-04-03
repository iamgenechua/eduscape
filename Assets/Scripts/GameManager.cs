using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [SerializeField] private GameObject player;
    public GameObject Player { get => player; }

    [SerializeField] private Fade fade;

    void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StartGame());

        // TODO: remove for final build
        AudioManager.Instance.PlaySoundFx("Test Speaker");
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(3f);
        fade.FadeIn();
        TutorialManager.Instance.StartTutorial();
    }

    public void RestartGame() {
        if (fade.IsFading) {
            return;
        }

        fade.FadeOutCompleteEvent.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        fade.FadeOut();
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

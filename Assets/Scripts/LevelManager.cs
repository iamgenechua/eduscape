using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;
    public static LevelManager Instance { get => _instance; }

    [SerializeField] private GameObject player;
    public GameObject Player { get => player; }

    [SerializeField] private FadePlayerView fade;

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
        StartCoroutine(StartLevel());
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator StartLevel() {
        yield return new WaitForSeconds(3f);
        fade.FadeIn();
        TutorialManager.Instance.StartTutorial();
    }

    public void RestartLevel() {
        UnityAction restart = () => {
            fade.FadeOutCompleteEvent.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            fade.FadeOut();
        };

        if (fade.IsFading) {
            StartCoroutine(GameManager.Instance.WaitForConditionBeforeAction(() => !fade.IsFading, restart));
        } else {
            restart();
        }
    }
}

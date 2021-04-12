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

    [SerializeField] private GameObject playerHead;
    public GameObject PlayerHead { get => playerHead; }

    [SerializeField] private GameObject playerBody;
    public GameObject PlayerBody { get => playerBody; }

    [SerializeField] private FadePlayerView fade;

    [SerializeField] private ProjectileNetDestroyer projectileNetDestroyer;
    public bool IsProjectileNetDestroyerEnabled {
        get => projectileNetDestroyer.isActiveAndEnabled;
        set => projectileNetDestroyer.gameObject.SetActive(value);
    }

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

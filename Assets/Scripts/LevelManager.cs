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

    [Space(10)]

    [SerializeField] private FadePlayerView fade;

    [SerializeField] private ProjectileNetDestroyer projectileNetDestroyer;

    [Space(10)]

    [SerializeField] private ShipController ship;

    [SerializeField] private DisplayScreen shipDisplay;

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

    private IEnumerator StartLevel() {
        AudioManager.Instance.MuteAllAudio();

        yield return new WaitForSeconds(3f);
        
        fade.FadeIn();
        AudioManager.Instance.UnmuteAllAudio(true);
        
        TutorialManager.Instance.StartTutorial();
    }

    public void CompleteLevel() {
        StartCoroutine(RunLevelCompletion());
    }

    private IEnumerator RunLevelCompletion() {
        shipDisplay.SetText("Whew. That was close!");
        yield return new WaitUntil(() => !shipDisplay.IsRollingOut);
        yield return new WaitForSeconds(5f);

        MusicManager.Instance.PlayVictoryMusic();
        shipDisplay.SetText("Head to the back of the ship to finish up.");

        IEnumerator dotAdder = AddDotsBeforeFinalMessage();
        StartCoroutine(dotAdder);

        yield return new WaitForSeconds(10.05f);

        StopCoroutine(dotAdder);
        shipDisplay.SetText("You did good ^U^");
        yield return new WaitForSeconds(1.25f);

        ship.UnlockSummaryAndButtons();
    }

    private IEnumerator AddDotsBeforeFinalMessage() {
        yield return new WaitUntil(() => !shipDisplay.IsRollingOut);
        while (true) {
            shipDisplay.GetComponentInChildren<TextRollout>().Text += ".";
            yield return new WaitForSeconds(2f);
        }
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

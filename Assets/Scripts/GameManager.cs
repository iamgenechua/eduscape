using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [SerializeField] private GameObject player;
    public GameObject Player { get => player; }

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
        AudioManager.Instance.PlaySoundFx("Test Speaker");
    }

    // Update is called once per frame
    void Update() {
        
    }
}

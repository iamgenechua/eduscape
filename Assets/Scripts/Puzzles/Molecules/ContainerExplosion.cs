using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerExplosion : MonoBehaviour {

    private MoleculesContainer container;
    private AudioSource audioSource;

    [SerializeField] private GameObject explosionPrefab;
    private bool isExploding = false;

    [SerializeField] private Light spotLight;
    [SerializeField] private GameObject switchShield;
    [SerializeField] private ExteriorGateSwitch gateSwitch;
    [SerializeField] private RationaleCanvas rationaleCanvas;

    void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        container = GetComponent<MoleculesContainer>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void StartExplosion() {
        if (isExploding) {
            return;
        }

        isExploding = true;
        Invoke(nameof(Explode), 3f);
    }

    public void Explode() {

        audioSource.Play();
        
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        spotLight.gameObject.SetActive(false);

        switchShield.SetActive(false);
        gateSwitch.StartRaise();

        rationaleCanvas.FadeIn();
        
        container.Destroy();
    }
}

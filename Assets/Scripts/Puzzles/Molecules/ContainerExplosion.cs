using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerExplosion : MonoBehaviour {

    private MoleculesContainer container;

    [SerializeField] private GameObject explosionPrefab;

    [Space(10)]

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip shatterSound;

    [Space(10)]

    [SerializeField] private Light spotLight;

    [Space(10)]

    [SerializeField] private GameObject switchShield;
    [SerializeField] private ExteriorGateSwitch gateSwitch;

    [Space(10)]

    [SerializeField] private FadeText rationaleCanvas;

    private bool isExploding = false;

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
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        audioSource.PlayOneShot(explosionSound);
        audioSource.PlayOneShot(shatterSound);

        spotLight.gameObject.SetActive(false);

        switchShield.SetActive(false);
        gateSwitch.StartRaise();

        rationaleCanvas.FadeIn();
        
        container.Destroy();
    }
}

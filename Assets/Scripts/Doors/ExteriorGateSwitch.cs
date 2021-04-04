using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorGateSwitch : MonoBehaviour {

    private AudioSource audioSource;
    
    [Tooltip("The local Y value to raise the switch to.")]
    [SerializeField] private float raiseToLocalYValue;

    [SerializeField] private float preRaisePauseLength = 1.5f;
    [SerializeField] private float raiseSpeed = 1f;

    [SerializeField] private GameObject objectToRaise;

    [Tooltip("Because we cannot move the actual switch, we move the dummy switch, then swap it out for the actual switch once in position.")]
    [SerializeField] private GameObject dummySwitch;
    [SerializeField] private GameObject actualSwitch;

    private bool isRising = false;

    void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Start() {

    }

    public void StartRaise() {
        StartCoroutine(Raise());
    }

    private IEnumerator Raise() {
        audioSource.Play();
        
        isRising = true;

        yield return new WaitForSeconds(preRaisePauseLength);

        while (objectToRaise.transform.localPosition.y < raiseToLocalYValue) {
            objectToRaise.transform.Translate(new Vector3(0, raiseSpeed * Time.deltaTime, 0));
            yield return null;
        }

        actualSwitch.SetActive(true);
        dummySwitch.SetActive(false);

        isRising = false;
    }
}

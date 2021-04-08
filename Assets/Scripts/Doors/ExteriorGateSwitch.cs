using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorGateSwitch : MonoBehaviour {

    [Tooltip("The local Y value to raise the switch to.")]
    [SerializeField] private float raiseToLocalYValue;

    [Space(10)]

    [SerializeField] private float preRaisePauseLength = 1.5f;
    [SerializeField] private float raiseSpeed = 1f;

    [Space(10)]

    [SerializeField] private GameObject objectToRaise;

    [Space(10)]

    [Tooltip("Because we cannot move the actual switch, we move the dummy switch, then swap it out for the actual switch once in position.")]
    [SerializeField] private GameObject dummySwitch;
    [SerializeField] private GameObject actualSwitch;

    [Space(10)]

    [SerializeField] private AudioSource raiseAudioSource;
    [SerializeField] private AudioClip raiseClip;
    [SerializeField] private AudioClip stopClip;

    private bool isRising = false;

    public void StartRaise() {
        StartCoroutine(Raise());
    }

    private IEnumerator Raise() {
        isRising = true;

        yield return new WaitForSeconds(preRaisePauseLength);

        raiseAudioSource.clip = raiseClip;
        raiseAudioSource.Play();

        while (objectToRaise.transform.localPosition.y < raiseToLocalYValue) {
            objectToRaise.transform.Translate(new Vector3(0, raiseSpeed * Time.deltaTime, 0));
            yield return null;
        }

        raiseAudioSource.clip = stopClip;
        raiseAudioSource.Play();

        actualSwitch.SetActive(true);
        dummySwitch.SetActive(false);

        isRising = false;
    }
}

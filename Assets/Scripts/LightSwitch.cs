using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    [Tooltip("The local Y value to raise the switch to.")]
    [SerializeField] private float raiseToLocalYValue;

    [SerializeField] private float preRaisePauseLength = 1.5f;
    [SerializeField] private float raiseSpeed = 1f;
    private bool isRising = false;

    void Start() {

    }

    public void StartRaise() {
        StartCoroutine(Raise());
    }

    private IEnumerator Raise() {
        isRising = true;

        yield return new WaitForSeconds(preRaisePauseLength);

        while (transform.localPosition.y < raiseToLocalYValue) {
            transform.Translate(new Vector3(0, raiseSpeed * Time.deltaTime, 0));
            yield return null;
        }

        isRising = false;
    }
}

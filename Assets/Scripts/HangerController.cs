using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HangerController : MonoBehaviour {

    [SerializeField] private Light[] lightSetFirst;
    [SerializeField] private Light[] lightSetSecond;

    [SerializeField] private Animator[] wallAnims;
    [SerializeField] private string wallAnimParam = "isOpen";

    // Start is called before the first frame update
    void Start() {
        DeactivateSign();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ActivateSign() {
        StartCoroutine(ActivateSignDramatic());
    }

    private IEnumerator ActivateSignDramatic() {
        yield return new WaitForSeconds(2f);

        foreach (Light light in lightSetFirst) {
            light.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Light light in lightSetSecond) {
            light.gameObject.SetActive(true);
        }
    }

    private void DeactivateSign() {
        foreach (Light light in lightSetFirst) {
            light.gameObject.SetActive(false);
        }

        foreach (Light light in lightSetSecond) {
            light.gameObject.SetActive(false);
        }
    }

    public void OpenWalls() {
        foreach (Animator wallAnim in wallAnims) {
            wallAnim.SetBool(wallAnimParam, true);
        }
    }

    public void CloseWalls() {
        foreach (Animator wallAnim in wallAnims) {
            wallAnim.SetBool(wallAnimParam, false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerController : MonoBehaviour {

    [SerializeField] private Light[] lights;
    [SerializeField] private Canvas signCanvas;

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
        yield return new WaitForSeconds(3f);

        foreach (Light light in lights) {
            light.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        signCanvas.gameObject.SetActive(true);
    }

    private void DeactivateSign() {
        foreach (Light light in lights) {
            light.gameObject.SetActive(false);
        }

        signCanvas.gameObject.SetActive(false);
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

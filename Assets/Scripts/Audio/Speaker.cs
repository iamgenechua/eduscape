using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource.Play();
    }
}

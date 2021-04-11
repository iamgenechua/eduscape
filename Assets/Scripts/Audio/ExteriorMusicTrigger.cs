using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorMusicTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Invoke(nameof(MusicManager.Instance.PlayExteriorMusic), 3f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            MusicManager.Instance.StopExteriorMusic(true);
        }
    }
}

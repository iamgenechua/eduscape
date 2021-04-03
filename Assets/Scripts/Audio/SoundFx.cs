using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundFx : Sound {

    [SerializeField] private AudioClip[] audioClips;

    /// <summary>
    /// Initialises the sound effect's settings.
    /// </summary>
    public override void InitialiseSound(AudioSource audioSource) {
        if (audioClips.Length == 0) {
            Debug.LogError($"Sound [{name}] has no audio clips set.");
            return;
        }

        audioSource.clip = audioClips[0];
        base.InitialiseSound(audioSource);
    }

    /// <summary>
    /// Plays the sound FX. If multiple audio clips are available, a random one is played.
    /// </summary>
    public override void Play() {
        if (audioClips.Length > 1) {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}

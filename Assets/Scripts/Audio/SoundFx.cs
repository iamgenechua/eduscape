using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundFx : Sound {

    [SerializeField] private SoundFxSource source;
    [SerializeField] private AudioClip[] audioClips;

    /// <summary>
    /// Initialises the sound effect's settings.
    /// </summary>
    public void InitialiseSoundFx() {
        if (audioClips.Length == 0) {
            Debug.LogError($"Sound [{name}] has no audio clips set.");
            return;
        }

        source.DefaultVolume = defaultVolume;
        InitialiseSound(source.GetComponent<AudioSource>(), audioClips[0]);
    }

    /// <summary>
    /// Plays the sound FX. If multiple audio clips are available, a random one is played.
    /// </summary>
    public override void Play() {
        if (audioClips.Length > 1) {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        }

        source.IsPlaying = true;
    }

    public override void Stop() {
        source.IsPlaying = false;
    }
}

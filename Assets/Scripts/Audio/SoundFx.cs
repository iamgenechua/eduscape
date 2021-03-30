using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundFx : Sound {

    [SerializeField] private AudioClip[] audioClips;

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <remarks>The source's clip is set to the first of the sound FX's available audio clips.</remarks>
    /// <param name="toSet">The AudioSource to set this sound's audio source to.</param>
    public override void SetAudioSource(AudioSource toSet) {
        base.SetAudioSource(toSet);

        if (audioClips.Length == 0) {
            Debug.LogError($"Sound [{name}] has no audio clips set.");
            return;
        }

        audioSource.clip = audioClips[0];
    }

    /// <summary>
    /// Plays the sound FX. If multiple audio clips are available, a random one is played.
    /// </summary>
    public override void Play() {
        if (audioClips.Length > 1) {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        }

        base.Play();
    }
}

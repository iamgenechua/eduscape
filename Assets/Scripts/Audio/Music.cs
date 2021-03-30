using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music : Sound {

    [SerializeField] private AudioClip audioClip;

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <remarks>The source's audio clip is set to the music's audio clip.</remarks>
    /// <param name="toSet">The AudioSource to set this sound's audio source to.</param>
    public override void SetAudioSource(AudioSource toSet) {
        base.SetAudioSource(toSet);
        audioSource.clip = audioClip;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmbientSound : Sound {

    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AmbientNoiseSource source;

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <remarks>The source's audio clip is set to the ambient sound's audio clip.</remarks>
    /// <param name="toSet">The AudioSource to set this sound's audio source to.</param>
    public override void SetAudioSource(AudioSource toSet) {
        base.SetAudioSource(toSet);
        audioSource.clip = audioClip;
    }

    /// <summary>
    /// Updates the ambient sound's volume and status based on the player's position.
    /// </summary>
    /// <param name="playerPosition">The player's Vector3 position.</param>
    public void UpdateAmbientSound(Vector3 playerPosition) {
        float distanceToPlayer = Vector3.Distance(source.transform.position, playerPosition);
        if (distanceToPlayer <= source.GetDefaultVolumeRadius()) {
            // maintain volume at default volume
            Volume = defaultVolume;
            if (!audioSource.isPlaying) {
                Play();
            }

            return;
        }

        if (distanceToPlayer >= source.GetTotalRadius()) {
            // exceeded total radius; stop ambient sound
            Stop();
            return;
        }

        // decrease volume
        Volume = (source.GetTotalRadius() - distanceToPlayer) / source.GetDecreasingVolumeDistance() * defaultVolume;
        if (!audioSource.isPlaying) {
            Play();
        }
    }
}

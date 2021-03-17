using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sound {

    protected AudioSource audioSource;

    [SerializeField] protected string name;
    public string Name { get => name; }

    [Range(0, 1)]
    [SerializeField] protected float defaultVolume;
    public float DefaultVolume { get => defaultVolume; }
    public float Volume { get => audioSource.volume; set => audioSource.volume = value; }

    [SerializeField] protected bool defaultLoopSetting;
    public bool DefaultLoopSetting { get => defaultLoopSetting; }
    public bool Loop { get => audioSource.loop; set => audioSource.loop = value; }

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <param name="toSet">The AudioSource to set this sound's audio source to.</param>
    public virtual void SetAudioSource(AudioSource toSet) {
        audioSource = toSet;
        Volume = defaultVolume;
        Loop = defaultLoopSetting;
    }

    /// <summary>
    /// Plays the sound.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void Stop() {
        audioSource.Stop();
    }
}

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

        audioSource.Play();
    }
}

[System.Serializable]
public class AmbientSound : Sound {

    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform source;
    [SerializeField] private float defaultVolumeRadius;
    [SerializeField] private float decreasingVolumeDistance;

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
    /// Plays the ambient sound.
    /// </summary>
    public override void Play() {
        audioSource.Play();
    }

    /// <summary>
    /// Updates the ambient sound's volume and status based on the player's position.
    /// </summary>
    /// <param name="playerPosition">The player's Vector3 position.</param>
    public void UpdateAmbientSound(Vector3 playerPosition) {
        float distanceToPlayer = Vector3.Distance(source.position, playerPosition);
        if (distanceToPlayer <= defaultVolumeRadius) {
            // maintain volume at default volume
            Volume = defaultVolume;
            if (!audioSource.isPlaying) {
                Play();
            }

            return;
        }

        float totalRadius = defaultVolumeRadius + decreasingVolumeDistance;
        if (distanceToPlayer >= totalRadius) {
            // exceeded total radius; stop ambient sound
            Stop();
            return;
        }

        // decrease volume
        Volume = (totalRadius - distanceToPlayer) / decreasingVolumeDistance * defaultVolume;
        if (!audioSource.isPlaying) {
            Play();
        }
    }
}

[System.Serializable]
public class Music : Sound {

    [SerializeField] private AudioClip audioClip;

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
    /// Plays the music.
    /// </summary>
    public override void Play() {
        audioSource.Play();
    }

    /// <summary>
    /// Pauses the music.
    /// </summary>
    public void Pause() {
        audioSource.Pause();
    }
}

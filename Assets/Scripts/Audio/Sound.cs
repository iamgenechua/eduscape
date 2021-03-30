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
    public virtual void Play() {
        audioSource.Play();
    }

    /// <summary>
    /// Pauses the sound.
    /// </summary>
    public void Pause() {
        audioSource.Pause();
    }

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void Stop() {
        audioSource.Stop();
    }
}

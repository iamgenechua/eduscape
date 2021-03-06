using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sound {

    protected AudioSource audioSource;

    [SerializeField] protected string name;
    public string Name { get => name; }

    [Range(0, 1)]
    [SerializeField] protected float defaultVolume = 1f;
    public float DefaultVolume { get => defaultVolume; }
    public float Volume { get => audioSource.volume; set => audioSource.volume = value; }

    [SerializeField] protected bool loop = false;
    public bool Loop { get => audioSource.loop; set => audioSource.loop = value; }

    public virtual void InitialiseSound(AudioSource audioSource) {
        this.audioSource = audioSource;
        Volume = defaultVolume;
        Loop = loop;
    }

    /// <summary>
    /// Plays the sound.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Pauses the sound.
    /// </summary>
    public virtual void Pause() {
        audioSource.Pause();
    }

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public virtual void Stop() {
        audioSource.Stop();
    }
}

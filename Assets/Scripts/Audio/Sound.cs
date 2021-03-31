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

    [SerializeField] protected bool playOnAwake = false;
    public bool PlayOnAwake { get => audioSource.playOnAwake; set => audioSource.playOnAwake = value; }

    protected void InitialiseSound(AudioSource audioSource, AudioClip audioClip) {
        this.audioSource = audioSource;
        audioSource.clip = audioClip;
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

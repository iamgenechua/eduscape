using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;
    public static AudioManager Instance { get => _instance; }

    [SerializeField] private Transform player;

    [SerializeField] private SoundFx[] soundFxs;
    [SerializeField] private Music[] musicTracks;

    /// <summary>
    /// The currently playing music.
    /// </summary>
    public Music CurrentMusic { get; private set; }

    void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        foreach (SoundFx soundFx in soundFxs) {
            soundFx.InitialiseSoundFx();
        }

        foreach (Music music in musicTracks) {
            music.InitialiseMusic(gameObject.AddComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Plays the sound FX with the given name.
    /// </summary>
    /// <param name="name">The name of the sound FX to play.</param>
    /// <param name="doFade">If true, fades the sound FX in. Else, it starts at default volume.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void PlaySoundFx(string name, bool doFade = false, float fadeTime = 1f) {
        SoundFx soundFx = System.Array.Find(soundFxs, soundFx => soundFx.Name == name);

        if (soundFx == null) {
            Debug.LogError($"SoundFx [{name}] not found in AudioManager.");
        }

        soundFx.Play();
        if (doFade) {
            StartCoroutine(RaiseVolume(soundFx, 0, soundFx.DefaultVolume, fadeTime));
        } else {
            soundFx.Volume = soundFx.DefaultVolume;
        }
    }

    /// <summary>
    /// Stops the sound FX with the given name.
    /// </summary>
    /// <param name="name">The name of the sound FX to stop.</param>
    /// <param name="doFade">If true, fades the sound FX out before stopping. Else, it simply stops.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void StopSoundFx(string name, bool doFade = false, float fadeTime = 1f) {
        SoundFx soundFx = System.Array.Find(soundFxs, soundFx => soundFx.Name == name);
        
        if (soundFx == null) {
            Debug.LogError($"SoundFx [{name}] not found in AudioManager.");
        }

        soundFx.Stop();
        if (doFade) {
            StartCoroutine(StopSoundWithFade(soundFx, fadeTime));
        } else {
            soundFx.Stop();
        }
    }

    /// <summary>
    /// Plays the music with the given name.
    /// </summary>
    /// <param name="name">The name of the music to play.</param>
    /// <param name="doFade">If true, fades the music in. Else, music starts at default volume.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void PlayMusic(string name, bool doFade = false, float fadeTime = 1f) {
        Music music = System.Array.Find(musicTracks, music => music.Name == name);

        if (music == null) {
            Debug.LogError($"Music [{name}] not found in AudioManager.");
            return;
        }

        CurrentMusic = music;
        music.Play();
        if (doFade) {
            StartCoroutine(RaiseVolume(music, 0, music.DefaultVolume, fadeTime));
        } else {
            music.Volume = music.DefaultVolume;
        }

    }

    /// <summary>
    /// Pauses the given music.
    /// </summary>
    /// <param name="music">The music to pause.</param>
    /// <param name="doFade">If true, fades the music out before pausing. Else, music simply pauses.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void PauseMusic(Music music, bool doFade = false, float fadeTime = 1f) {
        if (doFade) {
            StartCoroutine(PauseMusicWithFade(music, fadeTime));
        } else {
            music.Pause();
            CurrentMusic = null;
        }
    }


    /// <summary>
    /// Resumes the given paused music.
    /// </summary>
    /// <param name="music">The music to resume.</param>
    /// <param name="doFade">If true, fades the music in. Else, music simply resumes.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void ResumeMusic(Music music, bool doFade = false, float fadeTime = 1f) {
        if (!doFade) {
            music.Play();
            music.Volume = music.DefaultVolume;
            return;
        }

        music.Play();
        StartCoroutine(RaiseVolume(music, 0, music.DefaultVolume, fadeTime));
    }

    /// <summary>
    /// Stops the given music.
    /// </summary>
    /// <param name="music">The music to stop.</param>
    /// <param name="doFade">If true, fades the music out before stopping. Else, music simply stops.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void StopMusic(Music music, bool doFade = false, float fadeTime = 1f) {
        if (doFade) {
            StartCoroutine(StopSoundWithFade(music, fadeTime));
        } else {
            music.Stop();
            CurrentMusic = null;
        }
    }

    /// <summary>
    /// Fades out and then pauses the given music.
    /// </summary>
    /// <param name="music">The music to pause.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    /// <returns>IEnumerator, as this method is a coroutine.</returns>
    private IEnumerator PauseMusicWithFade(Music music, float fadeTime) {
        StartCoroutine(LowerVolume(music, music.Volume, 0, fadeTime));
        yield return new WaitUntil(() => music.Volume <= 0);
        PauseMusic(music);
    }

    /// <summary>
    /// Fades out and then stops the given sound.
    /// </summary>
    /// <param name="sound">The sound to stop.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    /// <returns>IEnumerator, as this method is a coroutine.</returns>
    private IEnumerator StopSoundWithFade(Sound sound, float fadeTime) {
        StartCoroutine(LowerVolume(sound, sound.Volume, 0, fadeTime));
        yield return new WaitUntil(() => sound.Volume <= 0);
        sound.Stop();
    }

    /// <summary>
    /// Raises the volume of the given sound.
    /// </summary>
    /// <param name="sound">The sound whose volume to raise.</param>
    /// <param name="fromVolume">The volume from which to raise the sound.</param>
    /// <param name="toVolume">The volume to raise the sound to.</param>
    /// <param name="raiseTime">The time taken to raise the volume.</param>
    /// <returns>IEnumerator, as this method is a coroutine.</returns>
    private IEnumerator RaiseVolume(Sound sound, float fromVolume, float toVolume, float raiseTime) {
        sound.Volume = fromVolume;
        while (sound.Volume < toVolume) {
            sound.Volume += (1 / raiseTime) * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Lowers the volume of the given sound.
    /// </summary>
    /// <param name="sound">The sound whose volume to lower.</param>
    /// <param name="fromVolume">The volume from which to lower the sound.</param>
    /// <param name="toVolume">The volume to lower the sound to.</param>
    /// <param name="lowerTime">The time taken to lower the volume.</param>
    /// <returns>IEnumerator, as this method is a coroutine.</returns>
    private IEnumerator LowerVolume(Sound sound, float fromVolume, float toVolume, float lowerTime) {
        sound.Volume = fromVolume;
        while (sound.Volume > toVolume) {
            sound.Volume -= (1 / lowerTime) * Time.deltaTime;
            yield return null;
        }
    }
}
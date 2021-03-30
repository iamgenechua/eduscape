using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private Transform player;

    [SerializeField] private SoundFx[] soundFxs;
    [SerializeField] private AmbientSound[] ambientSounds;
    [SerializeField] private Music[] musicTracks;

    public Music CurrentMusic { get; private set; } // the currently playing music

    void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        foreach (SoundFx soundFx in soundFxs) {
            soundFx.SetAudioSource(gameObject.AddComponent<AudioSource>());
        }

        foreach (AmbientSound ambientSound in ambientSounds) {
            ambientSound.SetAudioSource(gameObject.AddComponent<AudioSource>());
        }

        foreach (Music music in musicTracks) {
            music.SetAudioSource(gameObject.AddComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update() {
        ManageAmbientSounds();
    }

    /// <summary>
    /// Plays the sound FX with the given name.
    /// </summary>
    /// <param name="name">The name of the sound FX to play.</param>
    public void PlaySoundFx(string name) {
        foreach (SoundFx soundFx in soundFxs) {
            if (soundFx.Name == name) {
                soundFx.Play();
                return;
            }
        }

        Debug.LogError($"SoundFx [{name}] not found in AudioManager.");
    }

    /// <summary>
    /// Updates the statuses of all ambient sounds.
    /// </summary>
    private void ManageAmbientSounds() {
        foreach (AmbientSound ambientSound in ambientSounds) {
            ambientSound.UpdateAmbientSound(player.position);
        }
    }

    /// <summary>
    /// Plays the music with the given name.
    /// </summary>
    /// <param name="name">The name of the music to play.</param>
    /// <param name="doFade">If true, fades the music in. Else, music starts at normal volume.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void PlayMusic(string name, bool doFade = false, float fadeTime = 1f) {
        foreach (Music music in musicTracks) {
            if (music.Name == name) {
                CurrentMusic = music;
                music.Play();
                if (doFade) {
                    StartCoroutine(RaiseVolume(music, 0, music.DefaultVolume, fadeTime));
                } else {
                    music.Volume = music.DefaultVolume;
                }
                return;
            }
        }

        Debug.LogError($"Music [{name}] not found in AudioManager.");
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
            StartCoroutine(StopMusicWithFade(music, fadeTime));
        } else {
            music.Stop();
            CurrentMusic = null;
        }
    }

    /// <summary>
    /// Fades out and then stops the given music.
    /// </summary>
    /// <param name="music">The music to stop.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    /// <returns>IEnumerator, as this method is a coroutine.</returns>
    private IEnumerator StopMusicWithFade(Music music, float fadeTime) {
        StartCoroutine(LowerVolume(music, music.Volume, 0, fadeTime));
        yield return new WaitUntil(() => music.Volume <= 0);
        StopMusic(music);
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

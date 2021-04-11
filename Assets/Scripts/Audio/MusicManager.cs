using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private static MusicManager _instance;
    public static MusicManager Instance { get => _instance; }

    private void Awake() {
        // singleton
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }

    [SerializeField] private bool playMusic = true;

    [Space(10)]

    [SerializeField] private bool useSongForIntro = false;
    [SerializeField] private AudioSource[] introRadioAudioSources;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip introSong;
    [SerializeField] private AudioClip introEnd;

    public bool UseSongForIntro { get => useSongForIntro; }

    public void PlayIntroMusic(float fadeInLength) {
        if (!playMusic) {
            return;
        }

        foreach (AudioSource source in introRadioAudioSources) {
            source.loop = !useSongForIntro;
            source.clip = useSongForIntro ? introSong : introMusic;
            StartCoroutine(FadeInIntroMusic(source, fadeInLength));
        }
    }

    private IEnumerator FadeInIntroMusic(AudioSource source, float fadeInLength) {
        source.volume = 0f;
        source.Play();

        SoundFxSource sfxSource = source.GetComponent<SoundFxSource>();
        sfxSource.DoModulateVolume = false;

        float targetVolume = sfxSource.DefaultVolume;
        float elapsedTime = 0f;
        while (elapsedTime < fadeInLength) {
            source.volume += (targetVolume - source.volume) / (fadeInLength - elapsedTime) * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
        sfxSource.DoModulateVolume = true;
    }

    public void StopIntroMusic() {
        foreach (AudioSource source in introRadioAudioSources) {
            source.Stop();
            source.PlayOneShot(introEnd);
        }
    }

    public void PlayStationMusic() {
        if (!playMusic) {
            return;
        }

        AudioManager.Instance.PlayMusic("Station");
    }

    public void StopStationMusic() {
        if (playMusic && AudioManager.Instance.CurrentMusic?.Name == "Station") {
            AudioManager.Instance.StopMusic(AudioManager.Instance.CurrentMusic);
        }
    }

    public void PlayExteriorMusic() {
        if (!playMusic) {
            return;
        }

        AudioManager.Instance.PlayMusic("Exterior");
    }

    public void StopExteriorMusic(bool doFade) {
        if (AudioManager.Instance.CurrentMusic?.Name == "Exterior") {
            AudioManager.Instance.StopMusic(AudioManager.Instance.CurrentMusic, doFade, 5f);
        }
    }
}

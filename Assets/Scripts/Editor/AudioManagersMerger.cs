using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class AudioManagersMerger : MonoBehaviour {

    [MenuItem("Audio/Merge AudioManager Prefabs into Scene AudioManager")]
    private static void MergeAudioManagers() {
        Scene activeScene = SceneManager.GetActiveScene();

        bool isUserSure = EditorUtility.DisplayDialog(
            "Merging AudioManagers into Active Scene AudioManager",
            $"AudioManagers from other scenes are about to be merged into the AudioManager in the current active scene ({activeScene.name}). " +
            $"Are you sure you want to do this?",
            "Yes", "No");

        if (!isUserSure) {
            return;
        }

        string path = "Assets/Prefabs/Audio Managers";
        string[] audioManagerGuids = AssetDatabase.FindAssets("t:Object", new[] { path });

        List<SoundFx> sfxList = new List<SoundFx>();
        List<Music> musicList = new List<Music>();

        foreach (string guid in audioManagerGuids) {
            // use the GUID to get the asset's path, then use said path to get the prefab asset itself
            AudioManager audioManagerPrefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(AudioManager)) as AudioManager;
            if (audioManagerPrefab is null) {
                Debug.LogWarning($"Prefab without AudioManager component encountered in {path}");
                return;
            }

            sfxList.AddRange(audioManagerPrefab.SoundFxs);
            musicList.AddRange(audioManagerPrefab.MusicTracks);
        }

        // find the (first, but there should only be one) AudioManager in the active scene
        AudioManager audioManagerInScene = null;
        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects()) {
            AudioManager audioManager = gameObject.GetComponent<AudioManager>();
            if (audioManager != null) {
                audioManagerInScene = audioManager;
                break;
            }
        }

        if (audioManagerInScene is null) {
            Debug.LogWarning($"No AudioManager found in {activeScene.name}.");
            return;
        }

        // add merged SFX lists into the audio manager in the active scene
        sfxList.ToArray().CopyTo(audioManagerInScene.SoundFxs, audioManagerInScene.SoundFxs.Length);

        // add merged musics lists into the audio manager in the active scene
        musicList.ToArray().CopyTo(audioManagerInScene.MusicTracks, audioManagerInScene.MusicTracks.Length);
    }
}

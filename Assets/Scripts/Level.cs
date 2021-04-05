using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level {

    [SerializeField] private string levelName;
    [SerializeField] private bool isAvailable;

    public Scene Scene {
        get {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                Scene scene = SceneManager.GetSceneByBuildIndex(i);
                if (scene.name == levelName) {
                    return scene;
                }
            }

            throw new System.ArgumentException(
                $"{levelName} does not correspond to the name of a scene in the build settings.");
        }
    }
}

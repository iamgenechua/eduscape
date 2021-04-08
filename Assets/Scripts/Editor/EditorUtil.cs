using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorUtil {

    public static string ConvertPathArrToString(string[] path) {
        return string.Join("/", path);
    }

    public static string[] GetGameObjectComponentNames(GameObject obj) {
        Component[] components = obj.GetComponents<Component>();
        return System.Array.ConvertAll(components, component => component.GetType().Name);
    }

    public static EditorWindow GetProjectHierarchyWindow() {
        System.Type hierarchyType = System.Type.GetType("UnityEditor.SceneHierarchyWindow, UnityEditor");
        return (EditorWindow)Resources.FindObjectsOfTypeAll(hierarchyType)[0];
    }

    public static GameObject[] GetChildrenOfGameObject(GameObject obj) {
        GameObject[] children = new GameObject[obj.transform.childCount];
        for (int i = 0; i < children.Length; i++) {
            children[i] = obj.transform.GetChild(i).gameObject;
        }

        return children;
    }

    public static string[] GetPathToObject(GameObject obj) {
        List<string> pathToObj = new List<string>();

        Transform parent = obj.transform.parent;
        while (parent != null) {
            pathToObj.Insert(0, parent.name);
            parent = parent.transform.parent;
        }

        return pathToObj.ToArray();
    }

    public static GameObject FindTargetObjectInScene(GameObject[] objectsOnThisLevel, int levelInHierarchy, string[] targetPath) {
        bool isEndOfPath = levelInHierarchy >= targetPath.Length - 1;
        foreach (GameObject obj in objectsOnThisLevel) {
            if (obj.name == targetPath[levelInHierarchy]) {
                if (isEndOfPath) {
                    // the current level in the hierarchy matches the target object's level
                    return obj;
                }

                // continue search with object's children in next level
                return FindTargetObjectInScene(GetChildrenOfGameObject(obj), levelInHierarchy + 1, targetPath);
            }
        }

        return null;
    }
}

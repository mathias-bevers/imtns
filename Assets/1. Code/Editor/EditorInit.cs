using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CleanRoom
{
    [InitializeOnLoad]
    public class EditorInit
    {
        private const string BOOTUP = "_BOOTUP";

        static EditorInit()
        {
            EditorBuildSettingsScene bootupScene = Array.Find(EditorBuildSettings.scenes,
                s => s.path.Contains(BOOTUP));

            if (ReferenceEquals(null, bootupScene))
            {
                Debug.LogError("Could not find \"_BOOTUP\" scene, make sure it exists and" +
                               " is in the build settings");
                return;
            }

            SceneAsset sceneAsset =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(bootupScene.path);

            EditorSceneManager.playModeStartScene = sceneAsset;
            Debug.Log(bootupScene.path +" was set as the default start scene");
        }
    }
}
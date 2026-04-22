using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private static IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode,
            Action<string> callback)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            callback(sceneName);
        }

        public static void LoadScene(string sceneName, LoadSceneMode mode,
            Action<string> callback)
        {
            Instance.StartCoroutine(LoadSceneAsync(sceneName, mode, callback));
        }
    }
}
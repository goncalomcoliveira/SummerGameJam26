using System.Collections;
using GoncaloMCOliveira.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerService : PersistentSingleton<SceneManagerService> {
    
    private bool isLoading;

    #region Unity Lifecycle

    private void OnEnable() {
        SceneEvents.LoadScene += HandleLoadScene;
        SceneEvents.LoadSceneAdditive += HandleLoadSceneAdditive;
        SceneEvents.UnloadScene += HandleUnloadScene;
        SceneEvents.ReloadActiveScene += HandleReloadScene;
    }

    private void OnDisable() {
        SceneEvents.LoadScene -= HandleLoadScene;
        SceneEvents.LoadSceneAdditive -= HandleLoadSceneAdditive;
        SceneEvents.UnloadScene -= HandleUnloadScene;
        SceneEvents.ReloadActiveScene -= HandleReloadScene;
    }

    #endregion

    #region Event Handlers

    private void HandleLoadScene(string sceneName) {
        if (isLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName, LoadSceneMode.Single));
    }

    private void HandleLoadSceneAdditive(string sceneName) {
        if (isLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName, LoadSceneMode.Additive));
    }

    private void HandleUnloadScene(string sceneName) {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            return;

        SceneManager.UnloadSceneAsync(sceneName);
    }

    private void HandleReloadScene() {
        var active = SceneManager.GetActiveScene().name;
        HandleLoadScene(active);
    }

    #endregion

    #region Loading Logic

    private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode mode) {
        
        isLoading = true;
        SceneEvents.RaiseSceneLoadStarted(sceneName);

        var op = SceneManager.LoadSceneAsync(sceneName, mode);

        if (op == null) {
            
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError(
                $"[SceneManagerService] Failed to load scene '{sceneName}'. " +
                $"Is it added to Build Settings?"
            );
            #endif
            
            isLoading = false;
            yield break;
        }

        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }

        SceneEvents.RaiseSceneLoadCompleted(sceneName);
        isLoading = false;
    }

    #endregion
}

using System;

public static class SceneEvents {
    
    // Requests
    public static event Action<string> LoadScene;
    public static event Action<string> LoadSceneAdditive;
    public static event Action<string> UnloadScene;
    public static event Action ReloadActiveScene;

    // Lifecycle notifications
    public static event Action<string> SceneLoadStarted;
    public static event Action<string> SceneLoadCompleted;

    #region Raise Methods

    public static void RaiseLoadScene(string sceneName)
        => LoadScene?.Invoke(sceneName);

    public static void RaiseLoadSceneAdditive(string sceneName)
        => LoadSceneAdditive?.Invoke(sceneName);

    public static void RaiseUnloadScene(string sceneName)
        => UnloadScene?.Invoke(sceneName);

    public static void RaiseReloadActiveScene()
        => ReloadActiveScene?.Invoke();

    internal static void RaiseSceneLoadStarted(string sceneName)
        => SceneLoadStarted?.Invoke(sceneName);

    internal static void RaiseSceneLoadCompleted(string sceneName)
        => SceneLoadCompleted?.Invoke(sceneName);

    #endregion
}
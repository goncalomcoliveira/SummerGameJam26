using GoncaloMCOliveira.AudioSystem;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    [Header("Setup")]
    [SerializeField] private Animator animator;
    
    [Header("Scene")]
    [SerializeField] private string gameSceneName;

    [Header("Transition")]
    [SerializeField] private string transitionInName;
    [SerializeField] private string transitionOutName;
    
    [Header("Windows")]
    [SerializeField] private GameObject mainMenuWindow;
    [SerializeField] private GameObject optionsWindow;

    private void Start() {
        AudioEvents.MixerSettingsRetrieved?.Invoke();
        //TransitionManager.Instance?.TransitionOut(transitionInName);
    }
    
    public async void StartGame() {
        AudioEvents.MixerSettingsStored?.Invoke();
        /*
        await TransitionManager
            .Instance?
            .TransitionIn(transitionInName)!;
            */
        SceneEvents.RaiseLoadScene(gameSceneName);
    }
    
    public void OpenOptions() {
        animator.CrossFade("MoveToOptions", 0f);
    }
    
    public void CloseOptions() {
        animator.CrossFade("MoveToMainMenu", 0f);
    }

    public void QuitGame() {
        Application.Quit();
    }

}

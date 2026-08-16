using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using GoncaloMCOliveira.TransitionSystem;

public class MusaranhoSplashPlayer : MonoBehaviour {
    
    [Header("Input")]
    [SerializeField] private InputActionReference skipAction;
    [SerializeField] private float inputIgnoreTime = 0.2f;

    private VideoPlayer videoPlayer;
    private bool isTransitioning;
    private bool inputEnabled;

    private void Awake() {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnEnable() {
        skipAction.action.Enable();
        skipAction.action.performed += OnSkipPerformed;
    }

    private void OnDisable() {
        skipAction.action.performed -= OnSkipPerformed;
        skipAction.action.Disable();
    }

    private async void Start() {
        await Task.Delay((int)(inputIgnoreTime * 1000f));
        inputEnabled = true;
    }

    private void OnSkipPerformed(InputAction.CallbackContext ctx) {
        if (!inputEnabled || isTransitioning)
            return;

        HardCutSkip();
    }    

    private void OnVideoEnd(VideoPlayer vp) {
        if (!isTransitioning)
            StartTransitionAndLoad();
    }

    private void HardCutSkip() {
        isTransitioning = true;
        LoadNextSceneImmediate();
    }

    private async void StartTransitionAndLoad() {
        isTransitioning = true;

        if (TransitionManager.Instance != null) {
            //await TransitionManager.Instance.TransitionIn("CircleMask");
        }

        LoadNextSceneImmediate();
    }

    private void LoadNextSceneImmediate() {
        var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }
}
using UnityEngine;

public class UpgradeUIManager : MonoBehaviour {
    [SerializeField] private GameObject upgradeScreen;

    private void Start() {
        upgradeScreen.SetActive(false);

        GameManager.Instance.OnGameStateChanged +=
            HandleGameStateChanged;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameStateChanged -=
                HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState newState) {
        var shouldShow = newState == GameState.Upgrade;
        upgradeScreen.SetActive(shouldShow);
    }

    public void StartNextRound() {
        GameManager.Instance.StartNextRound();
    }
}
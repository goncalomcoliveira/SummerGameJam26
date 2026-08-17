using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Playing,
    RoundComplete,
    Upgrade,
    GameOver
}

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] private GameState currentState;
    
    [SerializeField] private RoundManager roundManager;

    [Header("Round")]
    [SerializeField] private int currentRound = 1;

    [Header("Currency")]
    [SerializeField] private int coins = 0;
    
    public GameState CurrentState => currentState;
    public int CurrentRound => currentRound;
    public int Coins => coins;
    private List<Enemy> activeEnemies = new List<Enemy>();

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnRoundChanged;
    
    private bool roundSpawningFinished;

    private void Awake() {
        
        // Singleton setup
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        StartGame();
    }
    
    private void OnEnable() {
        if (roundManager != null) {
            roundManager.OnRoundSpawningFinished +=
                HandleRoundSpawningFinished;
        }
    }

    private void OnDisable() {
        if (roundManager != null) {
            roundManager.OnRoundSpawningFinished -=
                HandleRoundSpawningFinished;
        }
    }
    
    private void HandleRoundSpawningFinished() {
        roundSpawningFinished = true;
        CheckRoundComplete();
    }

    public void StartGame() {
        
        currentRound = 1;
        coins = 0;

        OnRoundChanged?.Invoke(currentRound);
        OnCoinsChanged?.Invoke(coins);

        StartCurrentRound();
    }
    
    private void StartCurrentRound()
    {
        roundSpawningFinished = false;

        SetGameState(GameState.Playing);

        roundManager.StartRound(currentRound);
    }
    
    public void StartNextRound()
    {
        currentRound++;

        OnRoundChanged?.Invoke(currentRound);

        Debug.Log(
            "Starting Round " + currentRound
        );

        StartCurrentRound();
    }

    public void SetGameState(GameState newState) {
        currentState = newState;

        OnGameStateChanged?.Invoke(currentState);

        Debug.Log("Game State: " + currentState);
    }

    public void AddCoins(int amount) {
        coins += amount;

        OnCoinsChanged?.Invoke(coins);

        Debug.Log(
            "Coins: " + coins
        );
    }

    public bool SpendCoins(int amount) {
        if (coins < amount)
        {
            return false;
        }

        coins -= amount;

        OnCoinsChanged?.Invoke(coins);

        return true;
    }

    public void CompleteRound() {
        Debug.Log(
            "Round " + currentRound + " Complete!"
        );

        SetGameState(GameState.RoundComplete);

        // For now, immediately enter upgrades.
        StartUpgradePhase();
    }

    public void StartUpgradePhase() {
        SetGameState(GameState.Upgrade);
    }
    
    public void RegisterEnemy(Enemy enemy) {
        if (!activeEnemies.Contains(enemy)) {
            activeEnemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Enemy enemy) {
        activeEnemies.Remove(enemy);

        CheckRoundComplete();
    }

    private void CheckRoundComplete() {
        if (!roundSpawningFinished)
            return;
        if (currentState != GameState.Playing)
            return;
        if (activeEnemies.Count == 0) {
            CompleteRound();
        }
    }

    public void GameOver() {
        SetGameState(GameState.GameOver);

        Debug.Log("GAME OVER");
    }
}
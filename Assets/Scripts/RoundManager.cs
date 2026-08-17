using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Round Settings")]
    [SerializeField] private int baseEnemyCount = 3;
    [SerializeField] private int enemiesPerRound = 2;

    [Header("Spawn Settings")]
    [SerializeField] private float timeBetweenSpawns = 0.5f;

    private Coroutine currentSpawnRoutine;

    public event System.Action OnRoundSpawningFinished;
    
    public void StartRound(int roundNumber) {
        if (currentSpawnRoutine != null) {
            StopCoroutine(currentSpawnRoutine);
        }

        currentSpawnRoutine =
            StartCoroutine(
                SpawnRound(roundNumber)
            );
    }

    private IEnumerator SpawnRound(int roundNumber) {
        
        var enemyCount =
            baseEnemyCount +
            (roundNumber - 1) * enemiesPerRound;

        Debug.Log(
            "Spawning " +
            enemyCount +
            " enemies for Round " +
            roundNumber
        );

        for (var i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(
                timeBetweenSpawns
            );
        }

        currentSpawnRoutine = null;
        
        OnRoundSpawningFinished?.Invoke();
    }

    private void SpawnEnemy() {
        
        if (enemyPrefabs.Count == 0) {
            Debug.LogWarning(
                "No enemy prefabs assigned!"
            );
            return;
        }

        if (spawnPoints.Count == 0) {
            Debug.LogWarning(
                "No spawn points assigned!"
            );
            return;
        }

        var enemyPrefab =
            enemyPrefabs[
                Random.Range(
                    0,
                    enemyPrefabs.Count
                )
            ];

        var spawnPoint =
            spawnPoints[
                Random.Range(
                    0,
                    spawnPoints.Count
                )
            ];

        Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
    }
}
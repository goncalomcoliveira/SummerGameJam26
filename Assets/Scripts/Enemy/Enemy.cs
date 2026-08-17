using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {
    
    [Header("Coin Drop")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinDropAmount = 3;

    private Health health;

    private void Awake() {
        health = GetComponent<Health>();
    }

    private void OnEnable() {
        health.OnDeath += Die;
    }

    private void Start() {
        if (GameManager.Instance != null) {
            GameManager.Instance.RegisterEnemy(this);
        }
    }

    private void OnDisable() {
        if (health != null) {
            health.OnDeath -= Die;
        }
    }

    private void Die() {
        DropCoins();

        if (GameManager.Instance != null) {
            GameManager.Instance.UnregisterEnemy(this);
        }
    }

    private void DropCoins() {
        if (coinPrefab == null)
            return;

        for (var i = 0; i < coinDropAmount; i++) {
            Instantiate(
                coinPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}
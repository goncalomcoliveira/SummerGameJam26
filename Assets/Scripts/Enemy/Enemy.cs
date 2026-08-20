using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [Header("Coin Drop")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinDropAmount = 3;

    [Header("Difficulty Scaling")]
    [SerializeField] private bool scaleHealthWithRounds = true;
    [SerializeField] private float healthMultiplierPerRound = 0.1f;

    [SerializeField] private bool scaleCoinDropsWithRounds;
    [SerializeField] private int additionalCoinsPerRounds = 5;

    private Health health;
    private bool isRegistered;

    public int SpawnRound { get; private set; } = 1;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += Die;
    }

    private void Start()
    {
        SpawnRound = GetCurrentRound();

        ApplyRoundScaling();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy(this);
            isRegistered = true;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= Die;
        }

        // Useful later if enemies are destroyed or
        // disabled without going through Die().
        if (
            isRegistered &&
            GameManager.Instance != null
        )
        {
            GameManager.Instance.UnregisterEnemy(this);

            isRegistered = false;
        }
    }

    private void Die()
    {
        DropCoins();

        if (
            isRegistered &&
            GameManager.Instance != null
        )
        {
            GameManager.Instance.UnregisterEnemy(this);

            isRegistered = false;
        }

        Destroy(gameObject);
    }

    private void DropCoins()
    {
        if (coinPrefab == null)
            return;

        int finalCoinAmount =
            GetScaledCoinDropAmount();

        for (int i = 0; i < finalCoinAmount; i++)
        {
            Instantiate(
                coinPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }

    private int GetCurrentRound()
    {
        // We'll connect this properly once the
        // RoundManager is created.
        return 1;
    }

    private void ApplyRoundScaling()
    {
        if (!scaleHealthWithRounds)
            return;

        float multiplier =
            1f +
            (
                healthMultiplierPerRound *
                (SpawnRound - 1)
            );

        // We'll add a scaling method to Health
        // in the next step.
        health.ApplyMaxHealthMultiplier(
            multiplier
        );
    }

    private int GetScaledCoinDropAmount()
    {
        if (!scaleCoinDropsWithRounds)
            return coinDropAmount;

        int additionalCoinGroups =
            (SpawnRound - 1) /
            additionalCoinsPerRounds;

        return
            coinDropAmount +
            additionalCoinGroups;
    }
}
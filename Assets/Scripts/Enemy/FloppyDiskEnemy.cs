using UnityEngine;

public class FloppyDiskEnemy : MonoBehaviour
{
    public enum SpawnSide
    {
        Top,
        Bottom
    }

    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField]
    private Transform topProjectileSpawnPoint;

    [SerializeField]
    private Transform bottomProjectileSpawnPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileDamage = 10f;

    [Header("Spawn")]
    [SerializeField] private SpawnSide spawnSide;

    private EnemyFreeze enemyFreeze;
    private float attackTimer;

    private void Awake()
    {
        enemyFreeze =
            GetComponent<EnemyFreeze>();
    }

    private void Start()
    {
        spawnSide =
            Random.value > 0.5f
                ? SpawnSide.Top
                : SpawnSide.Bottom;

        attackTimer =
            attackCooldown;
    }

    private void Update()
    {
        if (
            enemyFreeze != null &&
            enemyFreeze.IsFrozen
        )
        {
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Shoot();

            attackTimer =
                attackCooldown;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
            return;

        Transform spawnPoint =
            GetProjectileSpawnPoint();

        if (spawnPoint == null)
            return;

        Vector2 direction =
            GetShootDirection();

        Projectile projectile =
            Instantiate(
                projectilePrefab,
                spawnPoint.position,
                Quaternion.identity
            );

        projectile.Initialize(
            direction,
            projectileSpeed,
            projectileDamage
        );
    }

    private Vector2 GetShootDirection()
    {
        switch (spawnSide)
        {
            case SpawnSide.Top:
                return Vector2.down;

            case SpawnSide.Bottom:
                return Vector2.up;
        }

        return Vector2.down;
    }

    private Transform GetProjectileSpawnPoint()
    {
        switch (spawnSide)
        {
            case SpawnSide.Top:
                return bottomProjectileSpawnPoint;

            case SpawnSide.Bottom:
                return topProjectileSpawnPoint;
        }

        return null;
    }

    public void SetSpawnSide(
        SpawnSide side
    )
    {
        spawnSide = side;
    }

    public void SetAttackSpeedMultiplier(
        float multiplier
    )
    {
        attackCooldown /= multiplier;
    }

    public void SetProjectileSpeedMultiplier(
        float multiplier
    )
    {
        projectileSpeed *= multiplier;
    }

    public void SetProjectileDamageMultiplier(
        float multiplier
    )
    {
        projectileDamage *= multiplier;
    }
}
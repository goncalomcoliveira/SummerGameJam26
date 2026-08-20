using UnityEngine;

public class ClippyEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform logo;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileDamage = 10f;

    private EnemyFreeze enemyFreeze;
    private float attackTimer;

    private void Awake()
    {
        enemyFreeze =
            GetComponent<EnemyFreeze>();
    }

    private void Start()
    {
        FindLogo();

        attackTimer = attackCooldown;
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

        if (logo == null)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Shoot();

            attackTimer = attackCooldown;
        }
    }

    private void FindLogo()
    {
        GameObject logoObject =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (logoObject != null)
        {
            logo = logoObject.transform;
        }
    }

    private void Shoot()
    {
        if (
            projectilePrefab == null ||
            projectileSpawnPoint == null ||
            logo == null
        )
        {
            return;
        }

        Vector2 direction =
            (
                logo.position -
                projectileSpawnPoint.position
            ).normalized;

        Projectile projectile =
            Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.identity
            );

        projectile.Initialize(
            direction,
            projectileSpeed,
            projectileDamage
        );
    }
}
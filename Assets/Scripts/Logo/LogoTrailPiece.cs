using System.Collections;
using UnityEngine;

public class LogoTrailPiece : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 2f;

    [SerializeField]
    private float damageMultiplier = 0.5f;

    [SerializeField]
    private float damageInterval = 0.5f;

    [Header("Detection")]
    [SerializeField]
    private LayerMask enemyLayer;

    private readonly Collider2D[] hits =
        new Collider2D[32];

    private Collider2D trailCollider;
    private LogoController logo;

    public void Initialize(LogoController logoController)
    {
        logo = logoController;

        trailCollider =
            GetComponent<Collider2D>();

        StartCoroutine(DamageRoutine());

        Destroy(gameObject, lifetime);
    }

    private IEnumerator DamageRoutine()
    {
        while (true)
        {
            DamageEnemies();

            yield return new WaitForSeconds(
                damageInterval
            );
        }
    }

    private void DamageEnemies() {
        
        if (trailCollider == null)
            return;

        var filter =
            new ContactFilter2D {
                useLayerMask = true,
                layerMask = enemyLayer
            };

        var hitCount =
            trailCollider.Overlap(
                filter,
                hits
            );

        var damage =
            logo.GetDamage() *
            damageMultiplier;

        for (var i = 0; i < hitCount; i++)
        {
            var hit = hits[i];

            if (hit == null)
                continue;

            var health =
                hit.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
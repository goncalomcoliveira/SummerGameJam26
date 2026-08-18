using System.Collections;
using UnityEngine;

public class LogoAura : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LogoController logo;

    [Header("Aura Settings")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float width = 4f;
    [SerializeField] private float height = 2f;
    
    [Tooltip("Multiplier of the Logo's damage.")]
    [SerializeField] private float damageMultiplier = 0.25f;
    [SerializeField] private float damageInterval = 0.5f;

    [Header("Visual")]
    [SerializeField] private GameObject auraVisual;

    [SerializeField] private LayerMask enemyLayer;

    private bool isUnlocked;

    public bool IsUnlocked => isUnlocked;

    private void Awake()
    {
        if (auraVisual != null)
        {
            auraVisual.SetActive(false);
        }
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;

        if (auraVisual != null)
        {
            auraVisual.SetActive(true);
        }

        StartCoroutine(
            AuraDamageRoutine()
        );

        Debug.Log("Logo Aura unlocked!");
    }

    private IEnumerator AuraDamageRoutine()
    {
        while (isUnlocked)
        {
            DamageEnemies();

            yield return new WaitForSeconds(
                damageInterval
            );
        }
    }

    private void DamageEnemies() {
        
        var size = new Vector2(
            width,
            height
        );

        var hits =
            Physics2D.OverlapBoxAll(
                transform.position,
                size,
                0f,
                enemyLayer
            );

        var auraDamage =
            logo.GetDamage() *
            damageMultiplier;

        foreach (var hit in hits) {
            var closestPoint =
                hit.ClosestPoint(transform.position);

            if (!IsInsideEllipse(closestPoint))
                continue;

            var health =
                hit.GetComponent<Health>();

            if (health != null) {
                health.TakeDamage(auraDamage);
            }
        }
    }
    
    private bool IsInsideEllipse(Vector2 position) {
        var localPosition =
            position - (Vector2)transform.position;

        var horizontalRadius = width / 2f;
        var verticalRadius = height / 2f;

        var value =
            (localPosition.x * localPosition.x) /
            (horizontalRadius * horizontalRadius)
            +
            (localPosition.y * localPosition.y) /
            (verticalRadius * verticalRadius);

        return value <= 1f;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(
                width,
                height,
                0f
            )
        );
    }
}
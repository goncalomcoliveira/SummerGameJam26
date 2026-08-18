using System.Collections;
using UnityEngine;

public class EnemyFreeze : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float freezeDuration = 3f;

    private Rigidbody2D enemyRigidbody;

    private Coroutine freezeRoutine;

    public bool IsFrozen { get; private set; }

    private void Awake()
    {
        enemyRigidbody =
            GetComponent<Rigidbody2D>();
    }

    public void Freeze()
    {
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }

        freezeRoutine =
            StartCoroutine(FreezeRoutine());
    }

    private IEnumerator FreezeRoutine()
    {
        IsFrozen = true;

        Vector2 previousVelocity = Vector2.zero;
        float previousAngularVelocity = 0f;

        if (enemyRigidbody != null)
        {
            previousVelocity =
                enemyRigidbody.linearVelocity;

            previousAngularVelocity =
                enemyRigidbody.angularVelocity;

            enemyRigidbody.linearVelocity =
                Vector2.zero;

            enemyRigidbody.angularVelocity =
                0f;

            enemyRigidbody.simulated = false;
        }

        yield return new WaitForSeconds(
            freezeDuration
        );

        if (enemyRigidbody != null)
        {
            enemyRigidbody.simulated = true;

            enemyRigidbody.linearVelocity =
                previousVelocity;

            enemyRigidbody.angularVelocity =
                previousAngularVelocity;
        }

        IsFrozen = false;
        freezeRoutine = null;
    }
}
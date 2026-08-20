using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ErrorPopupEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    private Vector2 direction;

    private Camera gameCamera;
    private Collider2D enemyCollider;
    private EnemyFreeze enemyFreeze;

    private void Awake()
    {
        gameCamera = Camera.main;

        enemyCollider =
            GetComponent<Collider2D>();

        enemyFreeze =
            GetComponent<EnemyFreeze>();
    }

    private void Start()
    {
        SetRandomDirection();
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

        Move();
        CheckScreenBounds();
    }

    private void Move()
    {
        transform.position +=
            (Vector3)(
                direction *
                speed *
                Time.deltaTime
            );
    }

    private void SetRandomDirection()
    {
        direction =
            Random.insideUnitCircle.normalized;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction = Vector2.right;
        }
    }

    private void CheckScreenBounds()
    {
        if (
            gameCamera == null ||
            enemyCollider == null
        )
        {
            return;
        }

        Vector3 bottomLeft =
            gameCamera.ViewportToWorldPoint(
                new Vector3(0f, 0f, 0f)
            );

        Vector3 topRight =
            gameCamera.ViewportToWorldPoint(
                new Vector3(1f, 1f, 0f)
            );

        Bounds bounds =
            enemyCollider.bounds;

        Vector3 position =
            transform.position;

        bool bounced = false;

        if (bounds.min.x < bottomLeft.x)
        {
            position.x +=
                bottomLeft.x -
                bounds.min.x;

            direction.x =
                Mathf.Abs(direction.x);

            bounced = true;
        }
        else if (bounds.max.x > topRight.x)
        {
            position.x -=
                bounds.max.x -
                topRight.x;

            direction.x =
                -Mathf.Abs(direction.x);

            bounced = true;
        }

        if (bounds.min.y < bottomLeft.y)
        {
            position.y +=
                bottomLeft.y -
                bounds.min.y;

            direction.y =
                Mathf.Abs(direction.y);

            bounced = true;
        }
        else if (bounds.max.y > topRight.y)
        {
            position.y -=
                bounds.max.y -
                topRight.y;

            direction.y =
                -Mathf.Abs(direction.y);

            bounced = true;
        }

        if (bounced)
        {
            direction.Normalize();
        }

        transform.position = position;
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (!other.CompareTag("Player"))
            return;

        BounceAwayFrom(
            other.transform.position
        );
    }

    private void BounceAwayFrom(
        Vector2 objectPosition
    )
    {
        Vector2 awayDirection =
            (
                (Vector2)transform.position -
                objectPosition
            ).normalized;

        if (awayDirection.sqrMagnitude < 0.01f)
        {
            SetRandomDirection();
            return;
        }

        direction = awayDirection;
    }
}
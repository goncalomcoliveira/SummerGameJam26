using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CookieEnemy : MonoBehaviour
{
    private enum MovementState
    {
        Following,
        Bouncing
    }

    [Header("References")]
    [SerializeField] private Transform logo;

    [Header("Following")]
    [SerializeField] private float followSpeed = 2f;

    [Header("Bouncing")]
    [SerializeField] private float bounceSpeed = 8f;
    [SerializeField] private float bounceDuration = 2f;

    [Header("Screen Bounds")]
    [SerializeField] private bool bounceOffScreenBounds = true;

    private MovementState currentState;

    private Vector2 bounceDirection;
    private float bounceTimer;

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
        FindLogo();

        currentState =
            MovementState.Following;
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

        switch (currentState)
        {
            case MovementState.Following:
                FollowLogo();
                break;

            case MovementState.Bouncing:
                Bounce();
                break;
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
            logo =
                logoObject.transform;
        }
    }

    private void FollowLogo()
    {
        Vector2 direction =
            (
                logo.position -
                transform.position
            ).normalized;

        transform.position +=
            (Vector3)(
                direction *
                followSpeed *
                Time.deltaTime
            );
    }

    private void Bounce()
    {
        transform.position +=
            (Vector3)(
                bounceDirection *
                bounceSpeed *
                Time.deltaTime
            );

        if (bounceOffScreenBounds)
        {
            CheckScreenBounds();
        }

        bounceTimer -= Time.deltaTime;

        if (bounceTimer <= 0f)
        {
            currentState =
                MovementState.Following;
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
                new Vector3(
                    0f,
                    0f,
                    0f
                )
            );

        Vector3 topRight =
            gameCamera.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    0f
                )
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

            bounceDirection.x =
                Mathf.Abs(
                    bounceDirection.x
                );

            bounced = true;
        }
        else if (bounds.max.x > topRight.x)
        {
            position.x -=
                bounds.max.x -
                topRight.x;

            bounceDirection.x =
                -Mathf.Abs(
                    bounceDirection.x
                );

            bounced = true;
        }

        if (bounds.min.y < bottomLeft.y)
        {
            position.y +=
                bottomLeft.y -
                bounds.min.y;

            bounceDirection.y =
                Mathf.Abs(
                    bounceDirection.y
                );

            bounced = true;
        }
        else if (bounds.max.y > topRight.y)
        {
            position.y -=
                bounds.max.y -
                topRight.y;

            bounceDirection.y =
                -Mathf.Abs(
                    bounceDirection.y
                );

            bounced = true;
        }

        if (bounced)
        {
            bounceDirection.Normalize();
        }

        transform.position =
            position;
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            currentState ==
            MovementState.Following
        )
        {
            if (
                other.CompareTag("Player")
            )
            {
                StartBounce(
                    other.transform.position
                );
            }

            return;
        }

        if (
            currentState ==
            MovementState.Bouncing
        )
        {
            BounceOffObject(
                other.transform.position
            );
        }
    }

    private void StartBounce(
        Vector2 hitSourcePosition
    )
    {
        bounceDirection =
            (
                (Vector2)transform.position -
                hitSourcePosition
            ).normalized;

        if (
            bounceDirection.sqrMagnitude <
            0.01f
        )
        {
            bounceDirection =
                Random.insideUnitCircle
                    .normalized;
        }

        bounceTimer =
            bounceDuration;

        currentState =
            MovementState.Bouncing;
    }

    private void BounceOffObject(
        Vector2 objectPosition
    )
    {
        Vector2 newDirection =
            (
                (Vector2)transform.position -
                objectPosition
            ).normalized;

        if (
            newDirection.sqrMagnitude <
            0.01f
        )
        {
            return;
        }

        bounceDirection =
            newDirection;
    }

    public void SetFollowSpeedMultiplier(
        float multiplier
    )
    {
        followSpeed *= multiplier;
    }
}
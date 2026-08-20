using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AOLMessengerEnemy : MonoBehaviour
{
    private enum TravelDirection
    {
        LeftToRight,
        RightToLeft
    }

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField]
    private float distanceOutsideScreen = 1f;

    [Header("Spawn Position")]
    [SerializeField] private float minimumY = -3f;

    [SerializeField] private float maximumY = 3f;

    [Header("Timing")]
    [SerializeField]
    private float delayBetweenRuns = 1f;

    [SerializeField]
    private float doorAnimationDelay = 0.5f;

    [Header("Door Timing")]
    [SerializeField]
    private float entranceDoorCloseOffset = 2f;

    [SerializeField]
    private float exitDoorOpenOffset = 2f;

    [Header("Doors")]
    [SerializeField] private AOLDoor leftDoor;

    [SerializeField] private AOLDoor rightDoor;

    private Camera gameCamera;
    private Collider2D enemyCollider;
    private EnemyFreeze enemyFreeze;

    private TravelDirection currentDirection;

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
        StartCoroutine(
            TravelLoop()
        );
    }

    private IEnumerator TravelLoop()
    {
        while (true)
        {
            ChooseDirection();

            yield return StartCoroutine(
                PerformTravel()
            );

            yield return new WaitForSeconds(
                delayBetweenRuns
            );
        }
    }

    private void ChooseDirection()
    {
        currentDirection =
            Random.value > 0.5f
                ? TravelDirection.LeftToRight
                : TravelDirection.RightToLeft;
    }

    private IEnumerator PerformTravel()
    {
        Bounds screenBounds =
            GetScreenBounds();

        float travelY =
            GetRandomSpawnY();

        // Move both doors to the same Y
        // that the AOL Messenger will use.
        SetDoorsYPosition(
            travelY
        );

        AOLDoor entranceDoor =
            GetEntranceDoor();

        AOLDoor exitDoor =
            GetExitDoor();

        Vector2 startPosition =
            GetStartPosition(
                screenBounds,
                travelY
            );

        Vector2 finalExitPosition =
            GetFinalExitPosition(
                screenBounds,
                travelY
            );

        // Start outside the screen.
        transform.position =
            startPosition;

        // He cannot be hit while waiting outside.
        SetDamageable(false);

        // Open the entrance door.
        if (entranceDoor != null)
        {
            entranceDoor.Open();
        }

        yield return new WaitForSeconds(
            doorAnimationDelay
        );

        // He can now be hit once he starts entering.
        SetDamageable(true);

        bool entranceDoorClosed = false;
        bool exitDoorOpened = false;

        while (
            Mathf.Abs(
                transform.position.x -
                finalExitPosition.x
            ) > 0.01f
        )
        {
            if (
                enemyFreeze == null ||
                !enemyFreeze.IsFrozen
            )
            {
                MoveHorizontally(
                    finalExitPosition
                );

                float distanceFromEntrance =
                    GetDistanceFromEntrance(
                        screenBounds
                    );

                float distanceToExit =
                    GetDistanceToExit(
                        screenBounds
                    );

                // Close the entrance door after
                // AOL has travelled far enough inside.
                if (
                    !entranceDoorClosed &&
                    distanceFromEntrance >=
                    entranceDoorCloseOffset
                )
                {
                    if (entranceDoor != null)
                    {
                        entranceDoor.Close();
                    }

                    entranceDoorClosed = true;
                }

                // Open the exit door before
                // AOL reaches it.
                if (
                    !exitDoorOpened &&
                    distanceToExit <=
                    exitDoorOpenOffset
                )
                {
                    if (exitDoor != null)
                    {
                        exitDoor.Open();
                    }

                    exitDoorOpened = true;
                }
            }

            yield return null;
        }

        // Make sure he ends exactly at
        // the final outside position.
        transform.position =
            new Vector3(
                finalExitPosition.x,
                travelY,
                transform.position.z
            );

        // He is now outside the arena.
        SetDamageable(false);

        // Safety check: ensure the entrance
        // door is closed.
        if (!entranceDoorClosed)
        {
            if (entranceDoor != null)
            {
                entranceDoor.Close();
            }
        }

        // Safety check: ensure the exit
        // door was opened.
        if (!exitDoorOpened)
        {
            if (exitDoor != null)
            {
                exitDoor.Open();
            }
        }

        // Give him a tiny amount of time
        // to completely pass the exit door.
        yield return new WaitForSeconds(
            0.1f
        );

        // Close the exit door behind him.
        if (exitDoor != null)
        {
            exitDoor.Close();
        }
    }

    private void MoveHorizontally(
        Vector2 targetPosition
    )
    {
        float horizontalDirection =
            Mathf.Sign(
                targetPosition.x -
                transform.position.x
            );

        transform.position +=
            new Vector3(
                horizontalDirection *
                movementSpeed *
                Time.deltaTime,

                0f,

                0f
            );
    }

    private void SetDoorsYPosition(
        float y
    )
    {
        if (leftDoor != null)
        {
            leftDoor.SetYPosition(y);
        }

        if (rightDoor != null)
        {
            rightDoor.SetYPosition(y);
        }
    }

    private AOLDoor GetEntranceDoor()
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return leftDoor;
        }

        return rightDoor;
    }

    private AOLDoor GetExitDoor()
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return rightDoor;
        }

        return leftDoor;
    }

    private Vector2 GetStartPosition(
        Bounds screenBounds,
        float y
    )
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return new Vector2(
                screenBounds.min.x -
                distanceOutsideScreen,
                y
            );
        }

        return new Vector2(
            screenBounds.max.x +
            distanceOutsideScreen,
            y
        );
    }

    private Vector2 GetFinalExitPosition(
        Bounds screenBounds,
        float y
    )
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return new Vector2(
                screenBounds.max.x +
                distanceOutsideScreen,
                y
            );
        }

        return new Vector2(
            screenBounds.min.x -
            distanceOutsideScreen,
            y
        );
    }

    private float GetRandomSpawnY()
    {
        return Random.Range(
            minimumY,
            maximumY
        );
    }

    private float GetDistanceFromEntrance(
        Bounds screenBounds
    )
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return
                transform.position.x -
                screenBounds.min.x;
        }

        return
            screenBounds.max.x -
            transform.position.x;
    }

    private float GetDistanceToExit(
        Bounds screenBounds
    )
    {
        if (
            currentDirection ==
            TravelDirection.LeftToRight
        )
        {
            return
                screenBounds.max.x -
                transform.position.x;
        }

        return
            transform.position.x -
            screenBounds.min.x;
    }

    private Bounds GetScreenBounds()
    {
        if (gameCamera == null)
        {
            return new Bounds(
                Vector3.zero,
                Vector3.one
            );
        }

        Vector3 bottomLeft =
            gameCamera.ViewportToWorldPoint(
                new Vector3(
                    0f,
                    0f,
                    -gameCamera.transform.position.z
                )
            );

        Vector3 topRight =
            gameCamera.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    -gameCamera.transform.position.z
                )
            );

        Vector3 center =
            (bottomLeft + topRight) / 2f;

        Vector3 size =
            topRight - bottomLeft;

        return new Bounds(
            center,
            size
        );
    }

    private void SetDamageable(
        bool value
    )
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled =
                value;
        }
    }

    public void SetSpeedMultiplier(
        float multiplier
    )
    {
        movementSpeed *= multiplier;
    }
}
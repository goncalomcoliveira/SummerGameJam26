using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private SelectionBoxVisual selectionBoxPrefab;

    [Header("Settings")]
    [SerializeField] private float dragThreshold = 0.2f;
    [SerializeField] private float minimumSelectionSize = 0.1f;

    private Vector2 startPosition;
    private bool isMouseDown;
    private bool isDragging;

    private SelectionBoxVisual activeSelectionBox;

    public event Action<Bounds> OnSelectionCompleted;
    public event Action<Vector2> OnClick;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        HandleMouseDown();
        HandleMouseHold();
        HandleMouseUp();
    }

    private void HandleMouseDown()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        startPosition = GetMouseWorldPosition();

        isMouseDown = true;
        isDragging = false;
    }

    private void HandleMouseHold()
    {
        if (!isMouseDown || !Mouse.current.leftButton.isPressed)
            return;

        Vector2 currentPosition =
            GetMouseWorldPosition();

        // Don't start dragging until the mouse
        // has moved far enough.
        if (!isDragging)
        {
            float distance =
                Vector2.Distance(
                    startPosition,
                    currentPosition
                );

            if (distance >= dragThreshold)
            {
                StartDragging();
            }
        }

        if (
            isDragging &&
            activeSelectionBox != null
        )
        {
            activeSelectionBox.UpdateBox(
                startPosition,
                currentPosition
            );
        }
    }

    private void HandleMouseUp()
    {
        if (!Mouse.current.leftButton.wasReleasedThisFrame)
            return;

        Vector2 endPosition =
            GetMouseWorldPosition();

        if (isDragging)
        {
            CompleteSelection(endPosition);

            StopDragging();
        }
        else
        {
            HandleClick(endPosition);
        }

        isMouseDown = false;
        isDragging = false;
    }

    private void StartDragging()
    {
        isDragging = true;

        activeSelectionBox = Instantiate(
            selectionBoxPrefab,
            startPosition,
            Quaternion.identity
        );
    }

    private void CompleteSelection(
        Vector2 endPosition
    )
    {
        Bounds selectionBounds =
            CreateSelectionBounds(
                startPosition,
                endPosition
            );

        // Ignore extremely small selections.
        if (
            selectionBounds.size.x <
                minimumSelectionSize ||
            selectionBounds.size.y <
                minimumSelectionSize
        )
        {
            return;
        }

        OnSelectionCompleted?.Invoke(
            selectionBounds
        );
    }

    private Bounds CreateSelectionBounds(
        Vector2 start,
        Vector2 end
    )
    {
        Vector2 min = new Vector2(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y)
        );

        Vector2 max = new Vector2(
            Mathf.Max(start.x, end.x),
            Mathf.Max(start.y, end.y)
        );

        Vector2 center =
            (min + max) * 0.5f;

        Vector2 size =
            max - min;

        return new Bounds(
            center,
            size
        );
    }

    private void StopDragging()
    {
        if (activeSelectionBox != null)
        {
            Destroy(
                activeSelectionBox.gameObject
            );

            activeSelectionBox = null;
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 worldPosition =
            gameCamera.ScreenToWorldPoint(
                new Vector3(
                    mouseScreenPosition.x,
                    mouseScreenPosition.y,
                    -gameCamera.transform.position.z
                )
            );

        return new Vector2(
            worldPosition.x,
            worldPosition.y
        );
    }

    private void HandleClick(Vector2 position)
    {
        OnClick?.Invoke(position);

        RaycastHit2D hit =
            Physics2D.Raycast(
                position,
                Vector2.zero
            );

        if (hit.collider == null)
            return;

        Coin coin =
            hit.collider.GetComponent<Coin>();

        if (coin != null)
        {
            coin.Collect();
        }
    }
}
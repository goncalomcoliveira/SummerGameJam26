using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSelector : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private SelectionBoxVisual selectionBoxPrefab;

    [Header("Settings")]
    [SerializeField] private float dragThreshold = 0.2f;

    private Vector2 startPosition;
    private bool isMouseDown;
    private bool isDragging;

    private SelectionBoxVisual activeSelectionBox;

    private void Update() {
        if (Mouse.current == null)
            return;

        HandleMouseDown();
        HandleMouseHold();
        HandleMouseUp();
    }

    private void HandleMouseDown() {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        startPosition = GetMouseWorldPosition();

        isMouseDown = true;
        isDragging = false;
    }

    private void HandleMouseHold() {
        if (!isMouseDown || !Mouse.current.leftButton.isPressed)
            return;

        var currentPosition = GetMouseWorldPosition();

        // Don't start dragging until the mouse has moved far enough.
        if (!isDragging) {
            var distance =
                Vector2.Distance(
                    startPosition,
                    currentPosition
                );

            if (distance >= dragThreshold) {
                StartDragging();
            }
        }

        if (isDragging && activeSelectionBox != null) {
            activeSelectionBox.UpdateBox(
                startPosition,
                currentPosition
            );
        }
    }

    private void HandleMouseUp() {
        if (!Mouse.current.leftButton.wasReleasedThisFrame)
            return;

        var endPosition = GetMouseWorldPosition();

        if (isDragging) {
            SelectObjects(
                startPosition,
                endPosition
            );

            StopDragging();
        }
        else {
            HandleClick(endPosition);
        }

        isMouseDown = false;
        isDragging = false;
    }

    private void StartDragging() {
        isDragging = true;

        activeSelectionBox = Instantiate(
            selectionBoxPrefab,
            startPosition,
            Quaternion.identity
        );
    }

    private void StopDragging() {
        if (activeSelectionBox != null) {
            Destroy(activeSelectionBox.gameObject);
            activeSelectionBox = null;
        }
    }

    private Vector2 GetMouseWorldPosition() {
        
        var mouseScreenPosition =
            Mouse.current.position.ReadValue();

        var worldPosition =
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

    private void HandleClick(Vector2 position) {
        var hit =
            Physics2D.Raycast(
                position,
                Vector2.zero
            );

        if (hit.collider == null)
            return;

        var coin =
            hit.collider.GetComponent<Coin>();

        if (coin != null) {
            coin.Collect();
        }
    }

    private void SelectObjects(
        Vector2 start,
        Vector2 end
    ) {
        var center = (start + end) / 2f;

        var size = new Vector2(
            Mathf.Abs(end.x - start.x),
            Mathf.Abs(end.y - start.y)
        );

        var hits =
            Physics2D.OverlapBoxAll(
                center,
                size,
                0f
            );

        foreach (var hit in hits) {
            Debug.Log(
                "Selected: " + hit.name
            );
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour {
    
    public Camera gameCamera;

    private void Start() {
        Cursor.visible = false;
    }
    
    private void Update() {
        UpdateCursorPosition();

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            HandleClick();
        }
    }

    private void UpdateCursorPosition() {
        
        var mousePosition = Mouse.current.position.ReadValue();

        var worldPosition =
            gameCamera.ScreenToWorldPoint(mousePosition);

        worldPosition.z = 0f;

        transform.position = worldPosition;
    }

    private void HandleClick() {
        
        var mousePosition = Mouse.current.position.ReadValue();

        var mouseWorldPosition =
            gameCamera.ScreenToWorldPoint(mousePosition);

        mouseWorldPosition.z = 0f;

        var hit = Physics2D.Raycast(
            mouseWorldPosition,
            Vector2.zero
        );

        if (hit.collider == null)
            return;

        var coin = hit.collider.GetComponent<Coin>();

        if (coin != null) {
            coin.Collect();
        }
    }
}
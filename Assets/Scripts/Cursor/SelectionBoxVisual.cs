using UnityEngine;

public class SelectionBoxVisual : MonoBehaviour {
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void UpdateBox(Vector2 startPosition, Vector2 currentPosition) {
        
        // Calculate the minimum and maximum corners
        var min = Vector2.Min(startPosition, currentPosition);
        var max = Vector2.Max(startPosition, currentPosition);

        // Calculate size
        var size = max - min;

        // Calculate center
        var center = min + size / 2f;

        // Move the visual to the center
        transform.position = center;

        // Resize the sliced sprite instead of scaling it
        spriteRenderer.size = size;
    }
}
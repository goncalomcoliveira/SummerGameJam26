using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A RawImage subclass that scrolls its UVs over time to simulate a moving texture.
/// Useful for animated UI backgrounds or effects.
/// </summary>
[RequireComponent(typeof(CanvasRenderer))]
public class ScrollingImage : RawImage {

    [Tooltip("Speed of UV scrolling per second")]
    [SerializeField] private Vector2 scrollSpeed = Vector2.zero;

    private void Update() {
        if (Application.isPlaying) {
            uvRect = new Rect(uvRect.position + scrollSpeed * Time.deltaTime, uvRect.size);
        }
    }
}
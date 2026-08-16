using UnityEngine;

/// <summary>
/// Applies a scrolling UV effect to the texture of a SpriteRenderer's material.
/// This component is ideal for creating looping backgrounds or moving visual effects on 2D sprites.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ScrollingSprite : MonoBehaviour {
    
    [Tooltip("Speed of UV scrolling per second")]
    [SerializeField] private Vector2 scrollSpeed = Vector2.zero;

    [Tooltip("Texture to assign at runtime (optional)")]
    [SerializeField] private Texture2D textureToUse;

    private string shaderName = "Custom/ScrollingSpriteShader"; 

    private Material _material;
    private Vector2 _offset;

    private void Awake() {
        var sr = GetComponent<SpriteRenderer>();

        // Create a new material instance using the scrolling shader
        var shader = Shader.Find(shaderName);
        _material = new Material(shader);

        // Assign texture if specified
        if (textureToUse != null)
            _material.mainTexture = textureToUse;

        // Apply material to SpriteRenderer
        sr.material = _material;
    }

    private void Update() {
        _offset += scrollSpeed * Time.deltaTime;
        _material.mainTextureOffset = _offset;
    }
}
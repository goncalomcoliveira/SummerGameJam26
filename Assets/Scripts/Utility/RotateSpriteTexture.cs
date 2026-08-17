using UnityEngine;

public class RotateSpriteTexture : MonoBehaviour {
    
    [SerializeField] private float rotationSpeed = 90f;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    private float rotation;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update() {
        rotation += rotationSpeed * Time.deltaTime;

        spriteRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat(
            "_Rotation",
            rotation * Mathf.Deg2Rad
        );

        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
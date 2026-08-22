using UnityEngine;

public class EnemyVisualVariants : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Variants")]
    [SerializeField] private Sprite[] variants;

    [Header("Settings")]
    [SerializeField] private bool chooseRandomVariantOnStart = true;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        if (chooseRandomVariantOnStart)
        {
            ChooseRandomVariant();
        }
    }

    public void ChooseRandomVariant()
    {
        if (
            spriteRenderer == null ||
            variants == null ||
            variants.Length == 0
        )
        {
            return;
        }

        int randomIndex =
            Random.Range(
                0,
                variants.Length
            );

        spriteRenderer.sprite =
            variants[randomIndex];
    }

    public void SetVariant(int index)
    {
        if (
            spriteRenderer == null ||
            variants == null ||
            variants.Length == 0
        )
        {
            return;
        }

        if (
            index < 0 ||
            index >= variants.Length
        )
        {
            return;
        }

        spriteRenderer.sprite =
            variants[index];
    }
}
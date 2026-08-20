using UnityEngine;

public class CursorAbilities : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BoxSelector boxSelector;

    [Header("Layers")]
    [SerializeField] private LayerMask coinLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask projectileLayer;

    private bool groupCollectUnlocked;
    private bool groupFreezeUnlocked;
    private bool deleteProjectileUnlocked;

    public bool GroupCollectUnlocked =>
        groupCollectUnlocked;

    public bool GroupFreezeUnlocked =>
        groupFreezeUnlocked;

    public bool DeleteProjectileUnlocked =>
        deleteProjectileUnlocked;

    private void OnEnable()
    {
        if (boxSelector != null)
        {
            boxSelector.OnSelectionCompleted +=
                HandleSelectionCompleted;

            boxSelector.OnClick +=
                HandleClick;
        }
    }

    private void OnDisable()
    {
        if (boxSelector != null)
        {
            boxSelector.OnSelectionCompleted -=
                HandleSelectionCompleted;

            boxSelector.OnClick -=
                HandleClick;
        }
    }
    
    private void HandleClick(Vector2 position)
    {
        if (deleteProjectileUnlocked)
        {
            DeleteProjectile(position);
        }
    }
    
    private void DeleteProjectile(
        Vector2 position
    )
    {
        Collider2D hit =
            Physics2D.OverlapPoint(
                position,
                projectileLayer
            );

        if (hit == null)
            return;

        Projectile projectile =
            hit.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Delete();
        }
    }

    private void HandleSelectionCompleted(
        Bounds selectionBounds
    )
    {
        if (groupCollectUnlocked)
        {
            CollectCoins(selectionBounds);
        }

        if (groupFreezeUnlocked)
        {
            FreezeEnemies(selectionBounds);
        }
    }

    public void UnlockGroupCollect()
    {
        if (groupCollectUnlocked)
            return;

        groupCollectUnlocked = true;

        Debug.Log(
            "Group Collect unlocked!"
        );
    }

    public void UnlockGroupFreeze()
    {
        if (groupFreezeUnlocked)
            return;

        groupFreezeUnlocked = true;

        Debug.Log(
            "Group Freeze unlocked!"
        );
    }

    public void UnlockDeleteProjectile()
    {
        if (deleteProjectileUnlocked)
            return;

        deleteProjectileUnlocked = true;

        Debug.Log(
            "Projectile Delete unlocked!"
        );
    }

    private void CollectCoins(
        Bounds selectionBounds
    )
    {
        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                selectionBounds.center,
                selectionBounds.size,
                0f,
                coinLayer
            );

        foreach (Collider2D hit in hits)
        {
            Coin coin =
                hit.GetComponent<Coin>();

            if (coin != null)
            {
                coin.Collect();
            }
        }
    }

    private void FreezeEnemies(
        Bounds selectionBounds
    )
    {
        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                selectionBounds.center,
                selectionBounds.size,
                0f,
                enemyLayer
            );

        foreach (Collider2D hit in hits)
        {
            EnemyFreeze enemyFreeze =
                hit.GetComponent<EnemyFreeze>();

            if (enemyFreeze != null)
            {
                enemyFreeze.Freeze();
            }
        }
    }
}
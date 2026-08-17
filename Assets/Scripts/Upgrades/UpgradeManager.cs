using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private LogoController logo;
    [SerializeField] private Health logoHealth;

    [Header("All Upgrades")]
    [SerializeField] private List<UpgradeData> allUpgrades;

    private HashSet<UpgradeType> purchasedUniqueUpgrades =
        new HashSet<UpgradeType>();

    private Dictionary<UpgradeType, int> upgradeLevels =
        new Dictionary<UpgradeType, int>();

    public event Action<UpgradeData> OnUpgradePurchased;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool PurchaseUpgrade(UpgradeData upgrade)
    {
        if (!CanPurchase(upgrade))
            return false;

        int cost = GetUpgradeCost(upgrade);

        bool spentCoins =
            GameManager.Instance.SpendCoins(cost);

        if (!spentCoins)
            return false;

        ApplyUpgrade(upgrade);

        OnUpgradePurchased?.Invoke(upgrade);

        return true;
    }

    public bool CanPurchase(UpgradeData upgrade)
    {
        if (upgrade == null)
            return false;

        // Cannot buy unique upgrades twice.
        if (IsUpgradePurchased(upgrade))
            return false;

        // Not enough coins.
        if (
            GameManager.Instance.Coins <
            GetUpgradeCost(upgrade)
        )
        {
            return false;
        }

        return true;
    }

    public bool IsUpgradePurchased(
        UpgradeData upgrade
    )
    {
        if (upgrade.isRepeatable)
            return false;

        return purchasedUniqueUpgrades.Contains(
            upgrade.upgradeType
        );
    }

    public int GetUpgradeCost(
        UpgradeData upgrade
    )
    {
        int level =
            GetUpgradeLevel(
                upgrade.upgradeType
            );

        return upgrade.baseCost +
               (
                   upgrade.costIncreasePerLevel *
                   level
               );
    }

    public int GetUpgradeLevel(
        UpgradeType upgradeType
    )
    {
        if (
            upgradeLevels.TryGetValue(
                upgradeType,
                out int level
            )
        )
        {
            return level;
        }

        return 0;
    }

    private void ApplyUpgrade(
        UpgradeData upgrade
    )
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.LogoDamage:

                logo.IncreaseDamage(
                    upgrade.value
                );

                break;

            case UpgradeType.LogoSpeed:

                logo.IncreaseSpeed(
                    upgrade.value
                );

                break;

            case UpgradeType.LogoHealth:

                logoHealth.IncreaseMaxHealth(
                    upgrade.value
                );

                break;

            case UpgradeType.LogoAura:

                UnlockLogoAura();

                break;

            case UpgradeType.LogoTrail:

                UnlockLogoTrail();

                break;

            case UpgradeType.LogoShield:

                UnlockLogoShield();

                break;

            case UpgradeType.CursorCooldown:

                UpgradeCursorCooldown(
                    upgrade.value
                );

                break;

            case UpgradeType.CursorGroupCollect:

                UnlockGroupCollect();

                break;

            case UpgradeType.CursorGroupFreeze:

                UnlockGroupFreeze();

                break;

            case UpgradeType.CursorDeleteProjectile:

                UnlockProjectileDelete();

                break;
        }

        IncreaseUpgradeLevel(upgrade);

        if (!upgrade.isRepeatable)
        {
            purchasedUniqueUpgrades.Add(
                upgrade.upgradeType
            );
        }
    }

    private void IncreaseUpgradeLevel(
        UpgradeData upgrade
    )
    {
        UpgradeType type =
            upgrade.upgradeType;

        int currentLevel =
            GetUpgradeLevel(type);

        upgradeLevels[type] =
            currentLevel + 1;
    }

    #region Upgrade Effects

    private void UnlockLogoAura()
    {
        Debug.Log("Logo Aura Unlocked!");
    }

    private void UnlockLogoTrail()
    {
        Debug.Log("Logo Trail Unlocked!");
    }

    private void UnlockLogoShield()
    {
        Debug.Log("Logo Shield Unlocked!");
    }

    private void UpgradeCursorCooldown(
        float amount
    )
    {
        Debug.Log(
            "Cursor cooldown upgraded by " +
            amount
        );
    }

    private void UnlockGroupCollect()
    {
        Debug.Log(
            "Group Collect Unlocked!"
        );
    }

    private void UnlockGroupFreeze()
    {
        Debug.Log(
            "Group Freeze Unlocked!"
        );
    }

    private void UnlockProjectileDelete()
    {
        Debug.Log(
            "Projectile Delete Unlocked!"
        );
    }

    #endregion
}   
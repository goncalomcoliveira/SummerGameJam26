using UnityEngine;

[CreateAssetMenu(
    fileName = "New Upgrade",
    menuName = "Game/Upgrade"
)]
public class UpgradeData : ScriptableObject
{
    [Header("Information")]
    public string upgradeName;

    [TextArea]
    public string description;

    [Header("Upgrade")]
    public UpgradeType upgradeType;

    [Header("Cost")]
    public int baseCost = 10;

    [Tooltip("Cost increases by this amount for each level.")]
    public int costIncreasePerLevel = 5;

    [Header("Repeatable")]
    public bool isRepeatable = true;

    [Header("Effect")]
    public float value = 1f;
}
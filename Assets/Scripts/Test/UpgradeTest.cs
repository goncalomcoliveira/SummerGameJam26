using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeTest : MonoBehaviour {
    
    [SerializeField] private UpgradeData testUpgrade;

    private void Update() {
        if (Keyboard.current.uKey.wasPressedThisFrame) {
            UpgradeManager.Instance.PurchaseUpgrade(testUpgrade);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class DamageTester : MonoBehaviour {
    
    [SerializeField] private Health targetHealth;
    
    private void Update() {
        if (Keyboard.current.hKey.wasPressedThisFrame) {
            targetHealth.TakeDamage(10f);
        }
    }
}
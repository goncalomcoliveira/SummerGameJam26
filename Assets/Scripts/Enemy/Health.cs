using System;
using UnityEngine;

public class Health : MonoBehaviour {
    
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action OnDeath;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        if (isDead)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(
        float amount,
        bool healAmount = true
    ) {
        maxHealth += amount;

        if (healAmount) {
            currentHealth += amount;

            currentHealth =
                Mathf.Min(
                    currentHealth,
                    maxHealth
                );
        }
    }

    private void Die() {
        if (isDead)
            return;

        isDead = true;

        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
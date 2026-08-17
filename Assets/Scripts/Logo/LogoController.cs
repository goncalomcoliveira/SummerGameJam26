using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class LogoController : MonoBehaviour {
    
    [Header("Stats")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;
    private Health health;

    [Header("Corner Power")]
    public float cornerPowerDuration = 3f;
    public float cornerDamageMultiplier = 2f;

    private Rigidbody2D rb;

    private Vector2 direction;
    private float cornerPowerTimer;

    public bool IsCornerPowered => cornerPowerTimer > 0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void Start() {
        direction = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate() {
        rb.linearVelocity = direction * speed;

        if (cornerPowerTimer > 0f) {
            cornerPowerTimer -= Time.fixedDeltaTime;
        }
    }
    
    public void IncreaseDamage(float amount) {
        damage += amount;
    }

    public void IncreaseSpeed(float amount) {
        speed += amount;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        HandleBounce(collision);

        var enemyHealth = collision.gameObject.GetComponent<Health>();

        if (enemyHealth != null) {
            enemyHealth.TakeDamage(GetDamage());
        }
    }

    void HandleBounce(Collision2D collision) {
        if (collision.contactCount == 0)
            return;

        ContactPoint2D contact = collision.GetContact(0);

        direction = Vector2.Reflect(
            direction,
            contact.normal
        ).normalized;
    }

    public float GetDamage() {
        
        if (IsCornerPowered) {
            return damage * cornerDamageMultiplier;
        }

        return damage;
    }
    
    public void ActivateCornerPower() {
        cornerPowerTimer = cornerPowerDuration;
    }

    public void ModifyDirection(Vector2 force) {
        direction = (direction + force).normalized;
    }

    public void SetDirection(Vector2 newDirection) {
        direction = newDirection.normalized;
    }
}
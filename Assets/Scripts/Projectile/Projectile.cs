using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;

    [Header("Targeting")]
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private bool rotateTowardsDirection;
    
    private Vector2 direction;
    private float speed;
    private float damage;
    
    private bool initialized;

    public void Initialize(Vector2 movementDirection, float projectileSpeed, float projectileDamage) {
        
        direction = movementDirection.normalized;

        speed = projectileSpeed;
        damage = projectileDamage;
        
        if (rotateTowardsDirection) {
            var angle =
                Mathf.Atan2(
                    direction.y,
                    direction.x
                ) *
                Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    angle
                );
        }

        initialized = true;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!initialized)
            return;

        transform.position +=
            (Vector3)(
                direction *
                speed *
                Time.deltaTime
            );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // Check whether this object is a valid target.
        if (((1 << other.gameObject.layer) & targetLayer.value) == 0) {
            return;
        }
        
        Debug.Log("HIT!!!");

        var health =
            other.GetComponent<Health>();

        if (health != null) {
            health.TakeDamage(damage);
        }

        Delete();
    }

    public void Delete() {
        Destroy(gameObject);
    }
}
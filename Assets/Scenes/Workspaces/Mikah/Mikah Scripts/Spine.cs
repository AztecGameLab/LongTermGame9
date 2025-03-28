using UnityEngine;

public class Spine : MonoBehaviour
{
    public int damage = 1;
    public float speed = 20f;
    public Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject saguaroTemp =GameObject.Find("Saguaro");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), saguaroTemp.GetComponent<Collider2D>());
        rb.linearVelocity = Vector2.right * PlayerMovement.moveDirection * speed;  // Ensure the spine is moving
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Check if the collider has a Health component
        if (other.TryGetComponent<Health>(out var health))
        {
            // Apply damage to the enemy
            health.ApplyDamage(damage, DamageType.PlayerThrow, gameObject);
        }

        // Destroy the spine projectile after it has collided
        Destroy(gameObject);
    }
}


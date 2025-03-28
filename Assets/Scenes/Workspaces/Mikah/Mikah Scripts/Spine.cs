using UnityEngine;

public class Spine : MonoBehaviour
{
    public int damage = 1;
    public float speed = 20f;
    public Rigidbody2D rb;

    private int direction = 1;

    public void SetDirection(int dir)
    {
        direction = dir;
        Debug.Log("Projectile received direction: " + direction);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ignore all collisions with player (and its children)
        GameObject player = GameObject.Find("Saguaro");
        if (player != null)
        {
            Collider2D[] playerColliders = player.GetComponentsInChildren<Collider2D>();
            Collider2D[] spineColliders = GetComponentsInChildren<Collider2D>();

            foreach (var playerCol in playerColliders)
            {
                foreach (var spineCol in spineColliders)
                {
                    Physics2D.IgnoreCollision(spineCol, playerCol);
                }
            }
        }

        // Use transform.right (respects rotation of throwPoint)
        rb.linearVelocity = transform.right * direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Health>(out var health))
        {
            health.ApplyDamage(damage, DamageType.PlayerThrow, gameObject);
            Destroy(gameObject);
        }
    }
}
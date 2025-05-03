using UnityEngine;

public class Attack2 : MonoBehaviour
{
    [SerializeField] private int damage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.ApplyDamage(damage, DamageType.Enemy, gameObject);
            }
            
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                //rb.linearVelocity = new Vector2(rb.linearVelocityX, 10);
                rb.AddForce(new Vector2(-10f, 5f), ForceMode2D.Impulse);
            }
        }
    }
}


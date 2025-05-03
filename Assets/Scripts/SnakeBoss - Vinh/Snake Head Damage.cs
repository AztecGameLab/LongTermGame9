using UnityEngine;

public class SnakeHeadDamage : MonoBehaviour
{
    
    [SerializeField] private int damage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.ApplyDamage(damage, DamageType.Enemy, gameObject);
            }
            
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            }
        }
    }
}

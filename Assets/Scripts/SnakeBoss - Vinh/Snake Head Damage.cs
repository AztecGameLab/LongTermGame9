using UnityEditor.Localization.Plugins.XLIFF.V12;
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
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 10);
            }
        }
    }
}

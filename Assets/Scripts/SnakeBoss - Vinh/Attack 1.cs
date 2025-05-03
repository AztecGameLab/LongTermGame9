using UnityEngine;

public class Attack1 : MonoBehaviour
{
    
    
    [SerializeField] private int damage;


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
                rb.AddForce(new Vector2(-1000, 500));
            }

        }
    }
}

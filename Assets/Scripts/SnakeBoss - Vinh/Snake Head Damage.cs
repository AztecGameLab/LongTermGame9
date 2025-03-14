using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SnakeHeadDamage : MonoBehaviour
{
    
    private Rigidbody2D rb;
    [SerializeField] private int damage;
    public Transform body;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.ApplyDamage(damage, DamageType.Enemy, gameObject);
            }
        }
    }
}

using System;
using UnityEngine;

public class ExampleProjectile : MonoBehaviour
{
    public int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Health>(out var health))
        {
            health.ApplyDamage(damage, DamageType.Enemy, gameObject);
        }
    }
}

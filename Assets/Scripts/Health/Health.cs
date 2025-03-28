using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthValue = 5;
    
    public int Value => healthValue;
    public bool IsDead => healthValue <= 0;

    // Only take damage from these types. 
    // Does not prevent `onDamage` from being invoked, only sets damage to zero.
    [SerializeField]
    private DamageType vulnerableDamageTypes;
    
    // Does not invoke after `onDeath`
    public UnityEvent<int, DamageType, GameObject> onDamage;
    
    public UnityEvent<GameObject> onDeath;
    
    public UnityEvent<int> onHealthChanged;

    private void Start()
    {
        onHealthChanged.Invoke(healthValue);
    }

    public virtual void ApplyDamage(int damage, DamageType damageType, GameObject attacker)
    {
        if (IsDead) return;
        
        if (!IsVulnerable(damageType))
        {
            damage = 0;
        }
        
        healthValue -= damage;
        
        if (healthValue <= 0)
        {
            healthValue = 0;
            onDeath.Invoke(attacker);
        }
        
        onHealthChanged.Invoke(healthValue);
        onDamage.Invoke(damage, damageType, attacker);
    }

    public virtual void AddHealth(int amount)
    {
        healthValue += amount;
        onHealthChanged.Invoke(healthValue);
    }

    public bool IsVulnerable(DamageType damageType)
    {
        return vulnerableDamageTypes.HasFlag(damageType);
    }
    
}
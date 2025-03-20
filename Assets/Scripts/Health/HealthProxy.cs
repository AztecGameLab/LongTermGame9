using UnityEngine;

public class HealthProxy : Health
{
    public Health targetHealth;
    
    public new int Value => targetHealth.Value;
    public new bool IsDead => targetHealth.Value <= 0;

    public override void ApplyDamage(int damage, DamageType damageType, GameObject attacker)
    {
        targetHealth.ApplyDamage(damage, damageType, attacker);
    }

    public override void AddHealth(int amount)
    {
        targetHealth.AddHealth(amount);
    }
    
}   
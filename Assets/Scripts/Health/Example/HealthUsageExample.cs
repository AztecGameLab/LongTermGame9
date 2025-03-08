using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthUsageExample : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Health health;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    public void OnDamage(int damage, DamageType damageType, GameObject attacker)
    {
        if (health.IsDead) return;
        if (spriteRenderer == null) return;
        
        Debug.Log("Attacked by " + attacker.name + " for " + damage + " damage");
        
        UniTask.Void(async () =>
        {
            spriteRenderer.color = Color.red;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            spriteRenderer.color = Color.white;
        });
    }
    
    public void OnDeath(GameObject attacker)
    {
        Debug.Log("Killed by " + attacker.name);
        
        if (spriteRenderer == null) return;
        
        spriteRenderer.color = Color.black;
    }
    
}
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SnakeBossTest : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("isDead");

    [SerializeField] private Animator animator;
    
    private SpriteRenderer spriteRenderer;
    private Health health;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    public void OnDamage(int damage, DamageType damageType, GameObject attacker)
    {
        // if (health.IsDead) return;
        if (spriteRenderer == null) return;

        if (damage <= 0) return;
        
        UniTask.Void(async () =>
        {
            spriteRenderer.color = Color.red;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            if (health.IsDead) return;
            spriteRenderer.color = Color.white;
        });
    }
    
    public void OnDeath(GameObject attacker)
    {
        // if (spriteRenderer == null) return;
        
        // spriteRenderer.color = new Color(0.45f, 0.45f, 0.45f);
        animator.SetBool(IsDead, true);
    }
    
}

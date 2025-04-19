using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

    public class Enemy : MonoBehaviour
    {
        private DamageType damageType;
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
            
            UniTask.Void(async () =>
            {
                spriteRenderer.color = Color.red;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                spriteRenderer.color = Color.white;
            });
        }
    
        public void OnDeath(GameObject attacker)
        {
            if (spriteRenderer == null) return;
        
            spriteRenderer.color = Color.black;
        }
    }


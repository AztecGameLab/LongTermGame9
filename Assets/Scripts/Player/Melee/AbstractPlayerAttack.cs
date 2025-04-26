using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Melee
{
    public abstract class AbstractPlayerAttack : MonoBehaviour
    {
        // Common fields shared bewteen player attacks
        [SerializeField] protected int enemyDamage;
        [SerializeField] protected float cooldownTime;
        [SerializeField] protected Animator animator;
        
        protected readonly HashSet<Health> Enemies = new();
        protected bool CanAttack = true;
        protected float CurrentCooldownTime;
        protected virtual int AnimatorHash => -1;

        // Check if the hitbox collides with an enemy
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Apply damage
            if (collision.TryGetComponent<Health>(out var health))
            {
                Enemies.Add(health);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Health>(out var health))
            {
                Enemies.Remove(health);
            }
        }

        protected virtual void CooldownTick()
        {
            // Handle cooldown
            if (CurrentCooldownTime > 0)
            {
                CurrentCooldownTime -= Time.deltaTime;
            }
            else if (!CanAttack)
            {
                CanAttack = true;
            }
        }

        protected virtual DamageType DamageType => DamageType.PlayerKick;

        public virtual void DealDamage(InputAction.CallbackContext context)
        {
            if (!context.performed || !CanAttack) return;

            CanAttack = false;
            CurrentCooldownTime = cooldownTime;
            
            animator.SetTrigger(AnimatorHash);
            
            foreach (var health in Enemies)
            {
                health.ApplyDamage(enemyDamage, DamageType, gameObject);
            }
        }

        private void Update()
        {
            CooldownTick();
        }
    }
}
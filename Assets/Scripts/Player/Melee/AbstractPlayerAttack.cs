using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Melee
{
    public abstract class AbstractPlayerAttack : MonoBehaviour
    {
        // Common fields shared bewteen player attacks
        [Tooltip("The amount of damage this attack should inflict on enemies.")]
        [SerializeField] protected int enemyDamage;
        [Tooltip("The duration of downtime between attacks (in seconds).")]
        [SerializeField] protected float cooldownTime;
        
        [Tooltip("The player character's Animator component.")]
        [SerializeField] protected Animator animator;

        [Tooltip("The player character's Audio Source component.")]
        [SerializeField] protected AudioSource audioSource;
        [Tooltip("The sound effect associated with this attack.")]
        [SerializeField] protected AudioClip audioClip;
        [Tooltip("The volume of the sound effect associated with this attack.")]
        [SerializeField] [Range(0, 1)] protected float audioVolume = 1f;

        // Override this variable with the return of an Animator.StringToHash().
        protected virtual int AnimatorHash => -1;
        // Override this variable with the desired DamageType enum.
        protected virtual DamageType DamageType => DamageType.PlayerKick;

        protected readonly HashSet<Health> Enemies = new();
        protected bool CanAttack = true;
        protected float CurrentCooldownTime;

        /**
         * Wrapper function to the SFX
         * associated with this attack.
         */
        protected void PlayAudio()
        {
            audioSource.PlayOneShot(audioClip, audioVolume);
            // audioSource.clip = audioClip;
            // audioSource.Play();
        }

        /**
         * Event trigger to start tracking an enemy
         * who has left the attack's collision box.
         */
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Apply damage
            if (collision.TryGetComponent<Health>(out var health))
            {
                Enemies.Add(health);
            }
        }

        /**
         * Event trigger to stop tracking an enemy
         * who has left the attack's collision box.
         */
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Health>(out var health))
            {
                Enemies.Remove(health);
            }
        }

        /**
         * Controls the behavior of the cooldown and any
         * of the cooldown's side effects (i.e. whether
         * the player should be allowed to attack).
         */
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

        /**
         * Callback function to deal damage to enemies within the
         * attack's collision box. Should be triggered with a
         * Player Input component.
         */
        public virtual void DealDamage(InputAction.CallbackContext context)
        {
            if (!context.performed || !CanAttack || Mathf.Approximately(Time.timeScale, 0.0f)) return;

            CanAttack = false;
            CurrentCooldownTime = cooldownTime;

            animator.SetTrigger(AnimatorHash);
            PlayAudio();

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
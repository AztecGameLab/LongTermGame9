using UnityEngine;
using UnityEngine.InputSystem;

namespace Attacks
{
    public class GroundStomp : AbstractPlayerAttack
    {
        private static readonly int GroundStomping = Animator.StringToHash("GroundStomping");
        protected override DamageType DamageType => DamageType.PlayerStomp;
        protected override int AnimatorHash => GroundStomping;
        private float audioInterval;

        [SerializeField] private AudioSource stompSound;
        [SerializeField] private PlayerMovementControl controller;

        private bool doStomp;

        protected override void CooldownTick()
        {
            if (CurrentCooldownTime > 0)
            {
                CurrentCooldownTime -= Time.deltaTime;
            }
            else if (CanAttack)
            {
                CanAttack = false;
            }

            if (CurrentCooldownTime < cooldownTime / 2)
            {
                doStomp = false;
                animator.SetBool(GroundStomping, false);
            }
            else if (CurrentCooldownTime < audioInterval - 0.5f && doStomp)
            {
                stompSound.Play();
                audioInterval -= 0.5f;
            }
        }

        public override void DealDamage(InputAction.CallbackContext context)
        {
            if (CanAttack || !context.performed || !controller.IsGrounded())
            {
                doStomp = false;
                if (CurrentCooldownTime > cooldownTime / 2)
                {
                    CurrentCooldownTime = cooldownTime / 2;
                }

                animator.SetBool(AnimatorHash, false);
                controller.AllowMovement = true;
                return;
            }

            animator.SetBool(AnimatorHash, true);
            stompSound.Play();
            controller.AllowMovement = false;
            CanAttack = true;
            CurrentCooldownTime = cooldownTime;
            audioInterval = cooldownTime;
            doStomp = true;

            foreach (var health in Enemies)
            {
                health.ApplyDamage(enemyDamage, DamageType, gameObject);
            }
        }
    }
}
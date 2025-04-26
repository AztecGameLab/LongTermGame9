using UnityEngine;

namespace Player.Melee
{
    public class FrontKick : AbstractPlayerAttack
    {
        private static readonly int Kick = Animator.StringToHash("FrontKick");
        protected override DamageType DamageType => DamageType.PlayerKick;
        protected override int AnimatorHash => Kick;
    }
}
using UnityEngine;

namespace Attacks
{
    public class TailWhip : AbstractPlayerAttack
    {
        private static readonly int Whip = Animator.StringToHash("TailWhip");
        protected override DamageType DamageType => DamageType.PlayerTailWhip;
        protected override int AnimatorHash => Whip;
    }
}
using UnityEngine;

public class AttackAnimationScript : StateMachineBehaviour
{
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Attacking, true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Attacking, false);
    }
}

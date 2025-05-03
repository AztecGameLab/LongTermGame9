using Player.Melee;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArtemisAnimationTesting : MonoBehaviour
{
    private static readonly int TailWhip = Animator.StringToHash("TailWhip");
    private static readonly int ThrowSpine = Animator.StringToHash("ThrowSpine");
    private static readonly int GroundStomping = Animator.StringToHash("GroundStomping");

    public Animator animator;
    public PlayerMovementControl controller;
    public GroundStomp stompScript;

    public void tailWhip(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        animator.SetTrigger(TailWhip);
    }

    public void throwSpine(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        animator.SetTrigger(ThrowSpine);
    }

    public void groundStomp(InputAction.CallbackContext context)
    {
        if (context.performed && controller.IsGrounded())
        {
            animator.SetBool(GroundStomping, true);
            controller.AllowMovement = false;
        } else if (context.canceled)
        {
            animator.SetBool(GroundStomping, false);
            controller.AllowMovement = true;
        }
    }
    
    
}
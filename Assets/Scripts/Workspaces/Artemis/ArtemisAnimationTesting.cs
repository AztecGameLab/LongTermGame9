using UnityEngine;
using UnityEngine.InputSystem;

public class ArtemisAnimationTesting : MonoBehaviour
{
    private static readonly int TailWhip = Animator.StringToHash("TailWhip");
    private static readonly int ThrowSpine = Animator.StringToHash("ThrowSpine");

    public Animator animator;

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
}
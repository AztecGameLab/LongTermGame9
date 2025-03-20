using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInputExample : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    private Rigidbody2D rb;
    private float moveInput;

    public EventReference moveSound;
    private EventInstance moveSoundInstance;
    public EventReference jumpSound;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSoundInstance = RuntimeManager.CreateInstance(moveSound);
    }

    private void Update()
    {
        rb.linearVelocityX = moveInput * moveSpeed;
        if (rb.linearVelocityX is > 0.1f or < -0.1f)
        {
            moveSoundInstance.getPlaybackState(out var state);
            if (state != PLAYBACK_STATE.PLAYING)
            {
                moveSoundInstance.start();
            }
        }
        else
        {
            moveSoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        RuntimeManager.PlayOneShot(jumpSound);
    }

    public void DestroySelf(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        Destroy(gameObject);
    }
    
}

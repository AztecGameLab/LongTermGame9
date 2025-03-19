using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInputExample : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    private Rigidbody2D rb;
    private float moveInput;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocityX = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void DestroySelf(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Destroy(gameObject);
        }
    }
    
}

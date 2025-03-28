using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    public static float moveDirection = 1f;  // Static variable to track the last movement direction (1 for right, -1 for left)
    
    private void Update()
    {
        // Get horizontal input (left/right arrow keys or A/D keys)
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Update the movement direction
        if (moveInput != 0)
        {
            moveDirection = Mathf.Sign(moveInput);  // 1 for right, -1 for left
        }

        // Move the player based on input
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);
        
        // Flip the player's sprite based on the movement direction
        transform.localScale = new Vector3(Mathf.Sign(moveInput), 1f, 1f);
    }
}
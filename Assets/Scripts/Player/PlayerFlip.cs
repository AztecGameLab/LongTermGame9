using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    
    private Rigidbody2D body;

    public bool startsFacingRight = true;
    public float flipDeadzone = 0.1f;
    
    private bool isFacingRight = true;
    public Vector2 ForwardVector2 => isFacingRight ? Vector2.right : Vector2.left;
    public Vector3 ForwardVector3 => isFacingRight ? Vector3.right : Vector3.left;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + ForwardVector3);
    }

    private bool DoesCharacterNeedToFlip()
    {
        if (startsFacingRight)
        {
            return body.linearVelocityX > flipDeadzone && !isFacingRight || body.linearVelocityX < -flipDeadzone && isFacingRight;
        }
        
        return body.linearVelocityX > flipDeadzone && isFacingRight || body.linearVelocityX < -flipDeadzone && !isFacingRight;
    }
    
    // Update is called once per frame
    private void Update()
    {
        var scale = transform.localScale;
        if (!DoesCharacterNeedToFlip()) return;
        
        isFacingRight = !isFacingRight;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

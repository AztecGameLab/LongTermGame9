using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementControl : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int DoubleJumping = Animator.StringToHash("DoubleJumping");
    
    /*
     * movement control behaviour
     *
     */

    //related to walking
    [Header("Walking")] 
    public float walkSpeed = 1.0f;
    public float gravity = 1.0f;

    public float accelerationMultiplier = 5.0f;
    public float accelerationPower = 1.5f;

    public float acceptableClimbSlope = 45f;
    public float groundFriction = 0.4f;

    private float walkInput;

    //related to jumping
    [Header("Jumping")] 
    public float jumpForce = 10.0f;

    [Header("Ground Check")] 
    public Vector2 groundCheckBoxSize = Vector2.one;
    public float groundCheckVerticalOffset;
    public LayerMask groundLayer;
    private float currentGroundAngle;
    private Vector2 currentGroundNormal = Vector2.zero;

    [Header("Animation")]
    public Animator animator;
    
    private Rigidbody2D body;

    private bool hasDoubleJump;
    private bool HasInput => !Mathf.Approximately(walkInput, 0.0f);

    private bool allowMovement = true;

    public bool AllowMovement
    {
        get => allowMovement;
        set
        {
            allowMovement = value;

            if (!allowMovement)
            {
                walkInput = 0;
                CheckJumpCut();
            }
        }
    }

    private bool wasGrounded;
    
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCheckVerticalOffset,
            groundLayer);
    }
    
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = gravity;

        if (body.sharedMaterial == null)
        {
            body.sharedMaterial = new PhysicsMaterial2D();
        }
    }
    
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            TrySetFriction(groundFriction);
            GroundedBehavior();
        }
        else
        {
            TrySetFriction(0.0f);
            AirBehavior();
        }
    }
    
    private void Update()
    {
        if (!hasDoubleJump && IsGrounded())
        {
            hasDoubleJump = true;
        }
        
        if (animator)
        {
            UpdateAnimations();
        }
    }
    
    private void TrySetFriction(float friction)
    {
        if (Mathf.Approximately(body.sharedMaterial.friction, friction)) return;
        
        var material = body.sharedMaterial;
        material.friction = friction;
        body.sharedMaterial = material;
    }

    private void UpdateAnimations()
    {
        if (animator.GetBool(Moving) != HasInput)
        {
            animator.SetBool(Moving, HasInput);
        }

        if (animator.GetBool(Grounded) != IsGrounded())
        {
            animator.SetBool(Grounded, IsGrounded());
        }

        if (!Mathf.Approximately(animator.GetFloat(VerticalSpeed), body.linearVelocityY))
        {
            animator.SetFloat(VerticalSpeed, body.linearVelocityY);
        }

        if (animator.GetBool(DoubleJumping) != !hasDoubleJump)
        {
            animator.SetBool(DoubleJumping, !hasDoubleJump);
        }
    }

    //currently primarily used for interactions with the ground
    private void OnCollisionStay2D(Collision2D col)
    {
        //get average of all (reasonably-sloped) contact points' normals, use it to determine ground angle
        int numFloorContacts = 0;
        float angleOfOverallGroundNormal = 0;
        foreach (ContactPoint2D point in col.contacts)
        {
            Vector2 thisNormal = point.normal;
            float angleFromFloor = Vector2.Angle(Vector2.up, thisNormal);
            if (angleFromFloor <= acceptableClimbSlope)
            {
                numFloorContacts++;
                if (thisNormal.x < 0)
                {
                    //counteract all angles being positive
                    angleFromFloor = -(angleFromFloor);
                }

                angleOfOverallGroundNormal += angleFromFloor;
            }
        }

        if (numFloorContacts >= 1)
        {
            angleOfOverallGroundNormal /= numFloorContacts;
        }

        //now that we have the angle of the ground we're touching, set our ground angle
        currentGroundAngle = angleOfOverallGroundNormal;


        currentGroundNormal.x = Mathf.Sin(currentGroundAngle * Mathf.Deg2Rad);
        currentGroundNormal.y = Mathf.Cos(currentGroundAngle * Mathf.Deg2Rad);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCheckVerticalOffset, groundCheckBoxSize);
    }

    //standing still or walking around. Includes slopes.
    private void GroundedBehavior()
    {
        //grounded walk/stand behavior:
        //get desired walk speed, current walk speed, and the distance between them
        //apply a force proportional to this deltaV
        //at an angle dependent on currentGroundAngle

        float desiredWalkSpeed = walkInput * walkSpeed;

        Vector2 currentVel = body.linearVelocity;
        float
            currentWalkSpeed = currentVel.magnitude; //could refine this by doin a dot-product with the ground parallel,
        //but it'd be almost exactly the same when on the ground anyway

        //make currentWalkSpeed directional
        if (currentVel.x < 0) currentWalkSpeed = -(currentWalkSpeed);

        float deltaV = desiredWalkSpeed - currentWalkSpeed;


        //informs us on the direction to push
        Vector2 groundParallel =
            new Vector2(currentGroundNormal.y,
                -(currentGroundNormal.x)); // rotates currentGroundNormal 90 degrees right
        //groundParallel has a velocity of 1 and points right (tilted to be parallel to the ground).

        //walk force magnitude = accelerationMultiplier * (deltaV)^accelerationPower
        float poweredDeltaV = Mathf.Pow(Mathf.Abs(deltaV), accelerationPower) * accelerationMultiplier;
        deltaV = (deltaV > 0) ? poweredDeltaV : -(poweredDeltaV);

        Vector2 walkForceVector = groundParallel * deltaV;

        body.AddForce(walkForceVector);

        //preventing sliding on hills:
        Vector2 uphill = (groundParallel.y > 0) ? groundParallel : -(groundParallel);
        body.AddForce(uphill * (uphill.y * Physics.gravity.magnitude * gravity));
        //uphill = uphill direction, uphill.y = from 0 to 1 depending on slope
    }

    //jumps up. 
    private void JumpBehavior()
    {
        currentGroundNormal = Vector2.zero;
        body.linearVelocityY = 0;
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //forces down, for when jump key is released mid-jump
    private void JumpCut()
    {
        // body.AddForce(Vector2.down * jumpCutForce, ForceMode2D.Impulse);
        if (body.linearVelocityY > 0)
        {
            body.linearVelocityY = 0;
        }
    }

    private void AirBehavior()
    {
        float desiredWalkSpeed = walkInput * walkSpeed;

        Vector2 currentVel = body.linearVelocity;
        float currentWalkSpeed = currentVel.x;

        float deltaV = desiredWalkSpeed - currentWalkSpeed;

        //walk force magnitude = accelerationMultiplier * (deltaV)^accelerationPower
        float poweredDeltaV = Mathf.Pow(Mathf.Abs(deltaV), accelerationPower) * accelerationMultiplier;
        deltaV = (deltaV > 0) ? poweredDeltaV : -(poweredDeltaV);

        Vector2 walkForceVector = new Vector2(deltaV, 0);

        body.AddForce(walkForceVector);
        
        currentGroundNormal = Vector2.zero;
    }

    private void CheckIfJumpAllowed()
    {
        if (IsGrounded())
        {
            JumpBehavior();
        } 
        else if (hasDoubleJump)
        {
            hasDoubleJump = false;
            JumpBehavior();
        }
    }

    private void CheckJumpCut()
    {
        if (!IsGrounded())
        {
            JumpCut();
        }
    }

    //input signals
    public void walk(InputAction.CallbackContext context)
    {
        if (!AllowMovement) return;
        
        walkInput = context.ReadValue<float>();
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (!AllowMovement) return;
        
        if (context.performed)
        {
            CheckIfJumpAllowed();
        }

        if (context.canceled)
        {
            CheckJumpCut();
        }
    }
}
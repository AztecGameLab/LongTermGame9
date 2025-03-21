using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementControl : MonoBehaviour
{
    /*
     * movement control behaviour
     *
     */

    /*
     * TO DO LIST:
     *
     * onGround boolean has inconsistent behavior. What should the conditions be for being on the ground or not?
     * currently it's set true when OnCollisionStay finds an appropriate ground collider
     * and set false when we jump.
     * (possibly: set it true/false by sending a raycast down like 0.2 units and checking for ground beneath us.)
     * >>> I modified the ground check to use a boxcast, inspired by a tutorial I found (here)[https://www.youtube.com/watch?v=P_6W-36QfLA]
     * >>> I did however keep the OnCollisionStay2D method for tracking the normals of the ground for movement purposes on the slopes.
     *
     * Jumping adds a weird tiny sideways velocity. I have no idea why but it feels bad. maybe forcibly set velocity.x to 0 after a jump?
     *                                                                                    ^feels like a bandaid fix
     * >>> I believe the jump weirdness was mostly a result of the onGround check being inconsistent, along with
     * >>> the movements being based on the magnitude of the velocity (i.e. would take the jump's upwards momentum as directional movement).
     *
     * how do I plan to deal with rotation on the ground? want to prevent tipping and make it not feel so weird and boxy. (reduce rotational damping?)
     * >>> I just locked the rotation in the rigidbody component, since we don't want it to rotate (it might look weird on slopes but that's alright for now)
     *
     * add in-air movement. Should probably be similarly snappy to on-ground movement. Maybe the same?
     *                                        (in which case I should separate it into a different method and use it in both)
     * >>> I kept it very similar, mostly just removed the bits that had it reacting to the ground.
     *
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

    private Rigidbody2D body;

    private bool hasDoubleJump;
    
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = gravity;

        if (body.sharedMaterial == null)
        {
            body.sharedMaterial = new PhysicsMaterial2D();
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

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCheckVerticalOffset,
            groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCheckVerticalOffset, groundCheckBoxSize);
    }
    
    private void Update()
    {
        if (!hasDoubleJump && IsGrounded())
        {
            hasDoubleJump = true;
        }
    }
    
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            if (!Mathf.Approximately(body.sharedMaterial.friction, groundFriction))
            {
                var material = body.sharedMaterial;
                material.friction = groundFriction;
                body.sharedMaterial = material;
            }
            GroundedBehavior();
        }
        else
        {
            if (!Mathf.Approximately(body.sharedMaterial.friction, 0.0f))
            {
                var material = body.sharedMaterial;
                material.friction = 0.0f;
                body.sharedMaterial = material;
            }
            AirBehavior();
        }
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
    }

    //TODO
    //jump should be allowed when grounded or a double jump is available
    private bool CheckIfJumpAllowed()
    {
        return IsGrounded() || hasDoubleJump;
    }

    //TODO
    //jump cut should be allowed if a jump has been performed and we are in the air without a jump cut having already been performed
    private bool CheckIfJumpCutAllowed()
    {
        return !IsGrounded();
    }

    //input signals
    public void walk(InputAction.CallbackContext context)
    {
        walkInput = context.ReadValue<float>();
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (context.performed && CheckIfJumpAllowed())
        {
            if (!IsGrounded()) // necessarily means they're using a double jump
            {
                hasDoubleJump = false;
            }

            JumpBehavior();
        }

        if (context.canceled && CheckIfJumpCutAllowed())
        {
            JumpCut();
        }
    }
}
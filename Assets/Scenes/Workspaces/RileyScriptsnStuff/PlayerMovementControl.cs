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
     * 
     * Jumping adds a weird tiny sideways velocity. I have no idea why but it feels bad. maybe forcibly set velocity.x to 0 after a jump?
     *                                                                                    ^feels like a bandaid fix
     *                                                                                    
     * how do I plan to deal with rotation on the ground? want to prevent tipping and make it not feel so weird and boxy. (reduce rotational damping?)
     * 
     * add in-air movement. Should probably be similarly snappy to on-ground movement. Maybe the same?
     *                                        (in which case I should separate it into a different method and use it in both)
     * 
     * 
     * 
     */

    //related to walking
    public float walkSpeed = 1.0f;
    public float gravity = 1.0f;

    public float accelerationMultiplier = 5.0f;
    public float accelerationPower = 1.5f;

    public float acceptableClimbSlope = 45f;
    private float currentGroundAngle = 0f;
    private Vector2 currentGroundNormal = Vector2.zero;

    private float walkInput;

    //related to jumping
    public float jumpForce = 10.0f;
    public float jumpCutForce = 5.0f;

    private bool jumpKeyDown;
    private bool jumpInputThisFrame;   //true on frames where the jump key goes down from being up
    private bool jumpReleaseThisFrame; // true on frames where the jump key goes up from being down
    private bool jumpInputPrevFrame;   //used in determination of the above

    private bool onGround;



    private Rigidbody2D body;

    
    void Start()
    {
        onGround = false;
        jumpKeyDown = false;
        jumpInputPrevFrame = false;
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = gravity;
    }

    //currently primarily used for interactions with the ground
    void OnCollisionStay2D(Collision2D col)
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
                if(thisNormal.x < 0)
                {
                    //counteract all angles being positive
                    angleFromFloor = -(angleFromFloor);
                }
                angleOfOverallGroundNormal += angleFromFloor;
            }
        }
        if (numFloorContacts >= 1)
        {
            onGround = true;
            angleOfOverallGroundNormal = angleOfOverallGroundNormal / (float)numFloorContacts;
        }

        //now that we have the angle of the ground we're touching, set our ground angle
        currentGroundAngle = angleOfOverallGroundNormal;

        

        currentGroundNormal.x = Mathf.Sin(currentGroundAngle * Mathf.Deg2Rad);
        currentGroundNormal.y = Mathf.Cos(currentGroundAngle * Mathf.Deg2Rad);
        
    }

    
    void FixedUpdate()
    {
        if (onGround) { groundedBehavior(); }
        else { airBehavior(); }

        checkJumpInput(); //this method must be performed exactly once per frame to function properly

        if (jumpInputThisFrame && checkIfJumpAllowed()) { jumpBehavior(); }
        if (jumpReleaseThisFrame && checkIfJumpCutAllowed()) { jumpCut(); }

    }

    //standing still or walking around. Includes slopes.
    private void groundedBehavior()
    {
        //grounded walk/stand behavior:
        //get desired walk speed, current walk speed, and the distance between them
        //apply a force proportional to this deltaV
        //at an angle dependent on currentGroundAngle

        float desiredWalkSpeed = walkInput * walkSpeed;

        Vector2 currentVel = body.linearVelocity;
        float currentWalkSpeed = currentVel.magnitude; //could refine this by doin a dot-product with the ground parallel,
                                                       //but it'd be almost exactly the same when on the ground anyway

        //make currentWalkSpeed directional
        if (currentVel.x < 0) currentWalkSpeed = -(currentWalkSpeed);

        float deltaV = desiredWalkSpeed - currentWalkSpeed;


        //informs us on the direction to push
        Vector2 groundParallel = new Vector2(currentGroundNormal.y, -(currentGroundNormal.x)); // rotates currentGroundNormal 90 degrees right
                                                                                               //groundParallel has a velocity of 1 and points right (tilted to be parallel to the ground).

        //walk force magnitude = accelerationMultiplier * (deltaV)^accelerationPower
        float poweredDeltaV = Mathf.Pow(Mathf.Abs(deltaV), accelerationPower) * accelerationMultiplier;
        deltaV = (deltaV > 0) ? poweredDeltaV : -(poweredDeltaV);

        Vector2 walkForceVector = groundParallel * deltaV;

        body.AddForce(walkForceVector);

        //preventing sliding on hills:
        Vector2 uphill = (groundParallel.y > 0) ? groundParallel : -(groundParallel);
        body.AddForce(uphill * uphill.y * Physics.gravity.magnitude * gravity);
        //uphill = uphill direction, uphill.y = from 0 to 1 depending on slope
    }

    //jumps up. 
    private void jumpBehavior()
    {
        onGround = false;
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //forces down, for when jump key is released mid-jump
    private void jumpCut()
    {
        body.AddForce(Vector2.down * jumpCutForce, ForceMode2D.Impulse);
    }

    private void airBehavior()
    {
        //untilting is necessary.
        //uncertain if I should add torques or lock rotation and manually force us upright.
        //for now, I'll go the torquing route for fear the forcing could look unnatural or clip us into the ground



    }



    
    //TODO
    //jump should be allowed when grounded or a double jump is available
    private bool checkIfJumpAllowed()
    {
        return true;
    }

    //TODO
    //jump cut should be allowed if a jump has been performed and we are in the air without a jump cut having already been performed
    private bool checkIfJumpCutAllowed()
    {
        return true;
    }

    //updates the "jump key was pressed/released on this frame" bools
    private void checkJumpInput()
    {
        jumpInputThisFrame = (!jumpInputPrevFrame && jumpKeyDown);   //true ONLY on the frame the jump key is pressed
        jumpReleaseThisFrame = (jumpInputPrevFrame && !jumpKeyDown); //true ONLY on the frame the jump key is released
        jumpInputPrevFrame = jumpKeyDown;

    }

    //input signals
    public void walk(InputAction.CallbackContext context)
    {
        Vector2 inpt = context.ReadValue<Vector2>();
        walkInput = inpt.x;
    }

    public void jump(InputAction.CallbackContext context)
    {
        jumpKeyDown = context.performed;

    }
}

using UnityEngine;

public class CameraSaguaroFollow : MonoBehaviour
{
    /*
     * This is just simple following behavior
     * I need to look into dynamic camera behavior
     * 
     */


    private Vector2 saguaroPosition;
    private Vector2 saguaroVelocity;
    private Vector2 pointToFollow;
    private Vector2 position;
    private Vector2 deltaPos;
    private Vector3 annoyingNecessaryVector3;

    [Header("Lookahead Distance")]
    [Tooltip("How far ahead of Saguaro the camera looks")]
    [SerializeField]
    public float lookahead = 0f;

    [Header("Y position")]
    [Tooltip("Camera Y-pos (set to -1 to follow Saguaro")]
    [SerializeField]
    public float setYPos = -1f;

    private bool staticY; //helper

    [Header("Y lookahead")]
    [Tooltip("Whether lookahead exists on the Y axis (if camera Y follows saguaro)")]
    public bool yLookahead = true;

    [Header("Tightness")]
    [Tooltip("How closely the camera follows, from 0 (camera never moves) to 1 (always exactly on saguaro or the lookahead point)")]
    [SerializeField]
    public float tightness = 0.01f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        annoyingNecessaryVector3 = Vector3.zero;
        annoyingNecessaryVector3.z = transform.position.z;
        //find Saguaro
    }

    void FixedUpdate()
    {
        //TODO: find saguaro's position and velocity each frame
        saguaroPosition.x = 0.0f;
        saguaroPosition.y = 0.0f;
        saguaroVelocity.x = 0.0f;
        saguaroVelocity.y = 0.0f;

        //find pointToFollow based on config values and saguaro's position and velocity
        staticY = (Mathf.Abs(setYPos + 1.0f) <= 0.01f);

        //lookahead calc no matter what, tightness no matter what
        //staticY and yLookahead are the trouble

        pointToFollow.x = saguaroPosition.x + (saguaroVelocity.x * lookahead);
        if (staticY)
        {
            pointToFollow.y = setYPos;
        } else if (!yLookahead)
        {
            pointToFollow.y = saguaroPosition.y;
        } else
        {
            pointToFollow.y = saguaroPosition.y + (saguaroVelocity.y * lookahead);
        }

        //now that the point to follow is set, go towards it, with strength dependent on tightness
        //get vector difference between pointToFollow and camera

        position.x = transform.position.x;
        position.y = transform.position.y;

        deltaPos = pointToFollow - position;

        //now move a percentage of that based on tightness
        position += deltaPos * tightness;

        //set transform of camera
        annoyingNecessaryVector3.x = position.x;
        annoyingNecessaryVector3.y = position.y;

        transform.position = annoyingNecessaryVector3;
        
    }
}

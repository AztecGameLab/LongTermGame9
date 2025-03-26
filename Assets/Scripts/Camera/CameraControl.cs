using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*
     * What this script does:
     * 
     * it keeps track of 2 positions: its own transform (just its x/y coords and size) and the Target Transform
     * its own transform will constantly move towards the target transform in a smooth, momentum-y way
     * the Target Transform will either be at a set of coordinates or follow saguaro.
     * 
     */



    //knowing saguaro's position is important for following him
    private GameObject saguaro;
    private Camera usCamera;

    public CamBehaviorType startingXBehavior = CamBehaviorType.FollowSaguaro;
    public CamBehaviorType startingYBehavior = CamBehaviorType.FollowSaguaro;

    public float startingXPos;
    public float startingYPos;

    public float startingSize = 5.0f;

    public float startingSpeed = 5.0f;

    private CamParameters defaultTargetPosInfo;

    private CamParameters targetPosInfo;

    //transform, current and target, are stored as Vector3's. x = x position, y = y position, z = size.
    private Vector3 currentPos;

    private Vector3 temp3D;

    private Vector3 targetPos;

    private float speed;


    private void Start()
    {
        saguaro = FindSaguaro();
        usCamera = GetComponent<Camera>();

        defaultTargetPosInfo = new CamParameters(startingXBehavior, startingYBehavior, startingXPos, startingYPos, startingSize, speed);
        targetPosInfo = new CamParameters(startingXBehavior, startingYBehavior, startingXPos, startingYPos, startingSize, speed);

        FindTargetPos();
        currentPos.x = targetPos.x;
        currentPos.y = targetPos.y;
        currentPos.z = targetPos.z;

        temp3D.z = transform.position.z;

        speed = startingSpeed;

    }


    private void FixedUpdate()
    {
        //1. set desired position based on current targetPosInfo
        FindTargetPos();



        //2. move camera towards desired position
        var between = targetPos - currentPos;
        currentPos += between * (Time.deltaTime * speed);

        SetTransform2DAndSize(currentPos);
    }

    private void SetTransform2DAndSize(Vector3 newTrans)
    {
        //temp3D.z is 'wherever the camera starts'
        temp3D.x = newTrans.x;
        temp3D.y = newTrans.y;

        transform.position = temp3D;

        usCamera.orthographicSize = newTrans.z;
    }

    private void FindTargetPos()
    {
        if (targetPosInfo.xBehavior == CamBehaviorType.FollowSaguaro)
        {
            targetPos.x = saguaro.transform.position.x;
        }
        else
        {
            targetPos.x = targetPosInfo.xPos;
        }

        if (targetPosInfo.yBehavior == CamBehaviorType.FollowSaguaro)
        {
            targetPos.y = saguaro.transform.position.y;
        }
        else
        {
            targetPos.y = targetPosInfo.yPos;
        }

        targetPos.z = targetPosInfo.size;
    }

                                        //NOTE FOR THOSE WHOSE RESPONSIBILITY IT IS TO PUT ALL OF THIS TOGETHER:
    //apologies, I'm not sure how we're identifying Saguaro. It's the same in CamTrigger, if (A) is wrong, replace it with the correct identification method.
    private static GameObject FindSaguaro()
    {
        
        return GameObject.FindGameObjectsWithTag("Player")[0]; //(A)
    }


    public void SetCamParameters(CamParameters newParams)
    {
        targetPosInfo = new CamParameters(newParams.xBehavior,
                                          newParams.yBehavior,
                                          newParams.xPos,
                                          newParams.yPos,
                                          newParams.size,
                                          newParams.speed);
        speed = newParams.speed;
    }

    public void ResetCamParameters()
    {
        targetPosInfo = new CamParameters(startingXBehavior,
                                          startingYBehavior,
                                          startingXPos,
                                          startingYPos,
                                          startingSize,
                                          startingSpeed);
        speed = startingSpeed;
    }
}

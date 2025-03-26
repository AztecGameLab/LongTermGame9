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
    GameObject Saguaro;
    Camera usCamera;

    public CamBehaviorType startingXBehavior = CamBehaviorType.followSaguaro;
    public CamBehaviorType startingYBehavior = CamBehaviorType.followSaguaro;

    public float startingXPos = 0.0f;
    public float startingYPos = 0.0f;

    public float startingSize = 5.0f;

    public float startingSpeed = 5.0f;

    CamParameters defaultTargetPosInfo;

    CamParameters targetPosInfo;

    //transform, current and target, are stored as Vector3's. x = x position, y = y position, z = size.
    Vector3 currentPos;

    Vector3 temp3D;

    Vector3 targetPos;

    private float speed;


    void Start()
    {
        Saguaro = findSaguaro();
        usCamera = GetComponent<Camera>();

        defaultTargetPosInfo = new CamParameters(startingXBehavior, startingYBehavior, startingXPos, startingYPos, startingSize, speed);
        targetPosInfo = new CamParameters(startingXBehavior, startingYBehavior, startingXPos, startingYPos, startingSize, speed);

        findTargetPos();
        currentPos.x = targetPos.x;
        currentPos.y = targetPos.y;
        currentPos.z = targetPos.z;

        temp3D.z = transform.position.z;

        speed = startingSpeed;

    }


    void FixedUpdate()
    {
        //1. set desired position based on current targetPosInfo
        findTargetPos();



        //2. move camera towards desired position
        Vector3 between = targetPos - currentPos;
        currentPos = currentPos + (between * Time.deltaTime * speed);

        setTransform2DAndSize(currentPos);
    }

    void setTransform2DAndSize(Vector3 newTrans)
    {
        //temp3D.z is 'wherever the camera starts'
        temp3D.x = newTrans.x;
        temp3D.y = newTrans.y;

        transform.position = temp3D;

        usCamera.orthographicSize = newTrans.z;
    }

    void findTargetPos()
    {
        if (targetPosInfo.xBehavior == CamBehaviorType.followSaguaro)
        {
            targetPos.x = Saguaro.transform.position.x;
        }
        else
        {
            targetPos.x = targetPosInfo.xPos;
        }

        if (targetPosInfo.yBehavior == CamBehaviorType.followSaguaro)
        {
            targetPos.y = Saguaro.transform.position.y;
        }
        else
        {
            targetPos.y = targetPosInfo.yPos;
        }

        targetPos.z = targetPosInfo.size;
    }

                                        //NOTE FOR THOSE WHOSE RESPONSIBILITY IT IS TO PUT ALL OF THIS TOGETHER:
    //apologies, I'm not sure how we're identifying Saguaro. It's the same in CamTrigger, if (A) is wrong, replace it with the correct identification method.
    GameObject findSaguaro()
    {
        
        return GameObject.FindGameObjectsWithTag("Player")[0]; //(A)
    }


    public void setCamParameters(CamParameters newParams)
    {
        targetPosInfo = new CamParameters(newParams.xBehavior,
                                          newParams.yBehavior,
                                          newParams.xPos,
                                          newParams.yPos,
                                          newParams.size,
                                          newParams.speed);
        speed = newParams.speed;
    }

    public void resetCamParameters()
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

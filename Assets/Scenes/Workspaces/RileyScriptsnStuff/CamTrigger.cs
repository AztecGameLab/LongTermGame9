using UnityEngine;

public class CamTrigger : MonoBehaviour
{

    
    public CamBehaviorType CameraXBehavior = CamBehaviorType.followSaguaro;
    public CamBehaviorType CameraYBehavior = CamBehaviorType.followSaguaro;

    public float CameraXCoord = 0.0f;
    public float CameraYCoord = 0.0f;

    public float CameraSize = 5.0f;

    public float cameraSpeed = 5.0f;

    private CamParameters passableParams;

    private CameraControl mainCam;

    void Start()
    {
        passableParams = new CamParameters(CameraXBehavior, CameraYBehavior, CameraXCoord, CameraYCoord, CameraSize, cameraSpeed);
        mainCam = Camera.main.gameObject.GetComponent<CameraControl>();
    }

                                        //NOTE TO THOSE WHOSE JOB IT IS TO PUT ALL THIS TOGETHER
    //apologies, I'm not sure how we're identifying Saguaro. It's the same in CameraControl, if (A) is wrong, replace it with the correct identification method.
    //same with the Camera.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) //(A)
        {
            mainCam.setCamParameters(passableParams);
        }
    }
}

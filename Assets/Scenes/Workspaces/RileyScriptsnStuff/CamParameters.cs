using UnityEngine;

public class CamParameters
{
    public CamBehaviorType xBehavior;
    public CamBehaviorType yBehavior;

    public float xPos;
    public float yPos;

    public float size;

    public float speed;

    public CamParameters(CamBehaviorType xBe= CamBehaviorType.followSaguaro,
                         CamBehaviorType yBe = CamBehaviorType.followSaguaro,
                         float xP= -99.9f,
                         float yP= -99.9f,
                         float si= -99.9f,
                         float sp= 5f)
    {
        xBehavior = xBe;
        yBehavior = yBe;
        xPos = xP;
        yPos = yP;
        size = si;
        speed = sp;
    }
}

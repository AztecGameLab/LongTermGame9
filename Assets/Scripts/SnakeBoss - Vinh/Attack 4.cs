using UnityEngine;

public class Attack4 : MonoBehaviour
{    
    [SerializeField] private GameObject snake;
    
    

    private Vector3 positionOffset;
    private readonly Vector3 hardCodedPosition = new(12.1f, -1.6f, -0.02544f);
    
    private Vector3 getOffsetVector(Vector3 position)
    {
        return position - positionOffset;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positionOffset = transform.position - hardCodedPosition;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.transform.position = new Vector2(gameObject.transform.position.x-0.2f, gameObject.transform.position.y);
        if (getOffsetVector(gameObject.transform.position).x < -32)
        {            
            snake.gameObject.SendMessage("BossFinishedAttack4");
            gameObject.SetActive(false);
        }
    }
}

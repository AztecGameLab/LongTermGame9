using Unity.VisualScripting;
using UnityEngine;

public class SnakeBossBehavior : MonoBehaviour
{

    private Rigidbody2D snakeBossBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("HIT");
            PlayerMoveTest script = collision.gameObject.GetComponent<PlayerMoveTest>();
            
            if (script != null) {
                script.funcKnock();
            }
        }

    }
}

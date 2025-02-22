using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 10;
    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter(new Vector2(Input.GetAxis("Horizontal"),0));
        if (Input.GetKeyDown(KeyCode.Space)) { 
            
        }
    }

    void moveCharacter(Vector2 direct) {
        transform.Translate(direct * speed * Time.deltaTime);
    }

    void jump() {
        rb2d.AddForce(new Vector2(0,1) * Time.deltaTime);
    
    }
}

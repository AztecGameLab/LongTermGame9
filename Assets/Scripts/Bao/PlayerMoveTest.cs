using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 10;
    private Rigidbody2D rb2d;
    private Collider2D collis;
    private bool canJump = true;
    private Coroutine knockbackCoroutine;
    private bool canMove = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        moveCharacter(new Vector2(Input.GetAxis("Horizontal"), 0));
        
        
        
        if (Input.GetKey(KeyCode.Space) && canJump) {
            canJump = false;
            jump();
        }
    }

    void moveCharacter(Vector2 direct) {
        if (canMove == true) {
            transform.Translate(direct * speed * Time.deltaTime);
        }
        
        
    }


    void jump() {
        rb2d.AddForce(Vector2.up * 200);
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground") {
            canJump = true;
        }
        
    }

    public void funcKnock() {
        if (knockbackCoroutine != null) {
            StopCoroutine(knockbackCoroutine);
        }

        knockbackCoroutine = StartCoroutine(WaitForForce());
    }

    IEnumerator WaitForForce() {
        rb2d.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        canMove = false;
        yield return new WaitUntil(() => rb2d.linearVelocity.magnitude < 0.1);
        canMove = true;
        knockbackCoroutine = null;

    }
}

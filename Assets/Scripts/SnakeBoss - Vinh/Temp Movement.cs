using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class tempmovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    public Transform objectTransform;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private int damage = 5;
    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    public void OnDamage(int damage, DamageType damageType, GameObject attacker)
    {
        Debug.Log("Attacked by " + attacker.name + " for " + damage + " damage");
        
        UniTask.Void(async () =>
        {
            
            if (health.IsDead)
            {
                spriteRenderer.color = Color.black;
            }
            else{
                spriteRenderer.color = Color.red;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                spriteRenderer.color = Color.white;
            }
        });
    }

    public void OnDeath(GameObject attacker)
    {
        Debug.Log("Snake Boss Defeated!");
        Debug.Log("Killed by " + attacker.name);
        Debug.Log("spriteRenderer: " + spriteRenderer == null);
        
        //if (spriteRenderer == null) return;
        
        spriteRenderer.color = Color.black;
        // Replace with scene transition logic
        //UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene"); 
    
    }

    void Update()
    {
        if(!health.IsDead)
        {

            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.ApplyDamage(damage, DamageType.PlayerKick, this.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    [SerializeField] private int screenSize;
    
    private int attack4Threshold = 5; // Set a default threshold
    private int attack4Counter;
    
    private Health health;
    private SpriteRenderer spriteRenderer;

    private int time;

    [SerializeField] private Transform hitBox;

    private int width, length;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        //time = Time.fixedTime;
    }

    public void FixedUpdate()
    {
        time++;
        if (time <= 200) return;
        time = 0; 
        Debug.Log("Current Health: " + health.Value);
        Attack();
        
    }

private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void OnDamage(int damage, DamageType damageType, GameObject attacker)
    {
        Debug.Log("Attacked by " + attacker.name + " for " + damage + " damage");
        
        
        UniTask.Void(async () =>
        {
            spriteRenderer.color = Color.red;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            spriteRenderer.color = health.IsDead ? Color.black: Color.white;
        });

        if (health.IsDead)
        {
            spriteRenderer.color = Color.black;
        }
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

    private int CheckAttack()
    {
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        float playerPositionX;
        if (tempPlayer != null)
        {
            playerPositionX = tempPlayer.transform.position.x;
            if (playerPositionX < -(screenSize / 2)) return 1;
            if (playerPositionX < 0) return 2;
            return 3;
        }
        Debug.Log("no player found");
        return 0;
        
    }
    
    public void Attack()
    {
        int temp = CheckAttack();
        
        if (attack4Counter == attack4Threshold)
            temp = 4;

        switch (temp)
        {
            case 1:
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
            case 4:
                Attack4();
                attack4Counter = 0; // Reset after attack 4
                break;
        }

        attack4Counter++;
    }

    private void Attack1()
    {
        Debug.Log("Snake Boss uses Attack 1!");
        
        // Implement attack logic here
    }

    private void Attack2()
    {
        Debug.Log("Snake Boss uses Attack 2!");
        // Implement attack logic here
    }

    private void Attack3()
    {
        Debug.Log("Snake Boss uses Attack 3!");
        // Implement attack logic here
    }

    private void Attack4()
    {
        Debug.Log("Snake Boss uses its powerful Attack 4!");
        // Implement attack logic here
    }
}
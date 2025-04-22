using System;
using Cysharp.Threading.Tasks;
using UnityEditor.Build;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    [SerializeField] private int screenSize;
    
    [SerializeField] private int attack4Threshold; // Set a default threshold
    private int attack4Counter;
    
    private Health health;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool finishedCutscene;

    [SerializeField] private int secondsInBetweenAttacks;
    
        
    [SerializeField] private GameObject snakeAttack4;
    [SerializeField] private GameObject snakeAttack1;
    
    private int timeForAttacks;

    private int attack1ActiveTime;
    private bool attack1Active;

    private bool bossBackup;

    private bool attackInProgress;
    
    private int width, length;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        //time = Time.fixedTime;
    }

    public void FixedUpdate()
    {
        if (attack1Active)
        {
            snakeAttack1.SetActive(true);
            attack1ActiveTime++;
            if (attack1ActiveTime >= 60)
            {
                snakeAttack1.SetActive(false);
                attackInProgress = false;
                attack1ActiveTime = 0;
                attack1Active = false;
            }
            
        }

        // This is for how the boss backs up to do the sweep across the floor attack
        if (bossBackup)
        {
            gameObject.transform.transform.position = new Vector2(gameObject.transform.position.x+0.2f, gameObject.transform.position.y);
            if (gameObject.transform.position.x > 12)
            {
                bossBackup = false;
                snakeAttack4.SetActive(true); 
                snakeAttack4.transform.position = new Vector2(12, -1.6f);
            }
        }
        //if the attack is done, bring back the boss
        else if (gameObject.transform.position.x > 5.5 && snakeAttack4.activeSelf == false)
        {
            gameObject.transform.transform.position = new Vector2(gameObject.transform.position.x-0.2f, gameObject.transform.position.y);
            attackInProgress = false;
        }

        if (!attackInProgress && finishedCutscene)
        {
            timeForAttacks++;
            if (timeForAttacks <= secondsInBetweenAttacks*60) return;
            timeForAttacks = 0; 
            Debug.Log("Current Health: " + health.Value);
            Debug.Log("Attack Counter: " + attack4Counter);
            Debug.Log("Attack 4 ACtive?" + snakeAttack4.activeSelf);
            Attack();
            
        }
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
            if (playerPositionX < -(screenSize / 2)) return 3;
            if (playerPositionX < 0) return 2;
            return 1;
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
                attack4Counter++;
                break;
            case 2:
                Attack2();
                attack4Counter++;
                break;
            case 3:
                Attack3();
                attack4Counter++;
                break;
            case 4:
                Attack4();
                attack4Counter = 0; // Reset after attack 4
                break;
        }

    }

    private void Attack1()
    {
        Debug.Log("Snake Boss uses Attack 1!");
        attackInProgress = true;
        attack1Active = true;
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
        attackInProgress = true;
        bossBackup = true;
        // Implement attack logic here
    }
}
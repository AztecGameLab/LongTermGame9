using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    [SerializeField] private float screenSize;
    
    [SerializeField] private int attack4Threshold; // Set a default threshold
    private int attack4Counter;
    private Animator animator;
    private Health health;
    private SpriteRenderer spriteRenderer;

    public bool finishedCutscene;

    [SerializeField] private int secondsInBetweenAttacks;
    
        
    [SerializeField] private GameObject snakeAttack1;
    [SerializeField] private GameObject snakeAttack2;
    [SerializeField] private Attack3 snakeAttack3;
    [SerializeField] private GameObject snakeAttack4;

    
    private int timeForAttacks;

    private bool bossComeUp4th;
    private bool bossBackup4th;

    private bool Attack1Forward;
    private bool Attack1Backward;
    
    private bool bossBackup2nd;
    private bool bossComeUp2nd;


    private bool attackInProgress;
    
    private int width, length;

    private Vector3 positionOffset;
    private readonly Vector3 hardCodedPosition = new(8.2f, -0.68f, -1.0f);
    
    private Vector3 getOffsetVector(Vector3 position)
    {
        return position - positionOffset;
    }
    

    public void setFinishedCutscene(bool b)
    {
        finishedCutscene = b;
    }
    
    public void Start()
    {
        positionOffset = transform.position - hardCodedPosition;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        //time = Time.fixedTime;
        
    }

    public void FixedUpdate()
    {
        if (health.IsDead) return;
        // This is for how the boss backs up to do the sweep across the floor attack
        
        // Debug.Log("Before Position: " + getOffsetVector(transform.position).x);

        if (Attack1Forward)
        {
            gameObject.transform.position = new Vector2(transform.position.x-0.3f, transform.position.y);
            if (getOffsetVector(transform.position).x <= 2.3)
            {
                Attack1Forward = false;
                Attack1Backward = true;
            }
        }
        else if (Attack1Backward)
        {
            gameObject.transform.position = new Vector2(transform.position.x+0.3f, transform.position.y);
            if (getOffsetVector(transform.position).x >= 8.2)
            {
                Attack1Backward = false;
            }
        }
        else if(bossBackup2nd)
        {
            // Debug.Log("Position: " + transform.position.x);
            gameObject.transform.position = new Vector2(transform.position.x+0.2f, transform.position.y);
            
            //gameObject.transform.position.Set(gameObject.transform.position.x+0.2f,gameObject.transform.position.y,gameObject.transform.position.z);
            // Debug.Log("AFTER Position: " + transform.position.x);
            if (getOffsetVector(gameObject.transform.position).x > 13)
            {
                bossBackup2nd = false;
                animator.Play("Snake Bite Close");
                //animator.SetTrigger("Snake2ndAttack");
                // call animate
            }
        }//if the attack is done, bring back the boss
        else if (bossComeUp2nd)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x-0.2f, gameObject.transform.position.y);
            if (getOffsetVector(gameObject.transform.position).x <= 8.2)
            {
                attackInProgress = false;
                bossComeUp2nd = false;
            }
        }
        // This is for how the boss backs up to do the sweep across the floor attack
        else if (bossBackup4th)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x+0.2f, gameObject.transform.position.y);
            if (getOffsetVector(gameObject.transform.position).x > 18)
            {
                bossBackup4th = false;
                snakeAttack4.SetActive(true); 
                snakeAttack4.transform.position = new Vector3(18.0f, -1.6f, 0.0f) + positionOffset;
            }
        }//if the attack is done, bring back the boss
        else if (bossComeUp4th)
        {
            gameObject.transform.transform.position = new Vector2(gameObject.transform.position.x-0.2f, gameObject.transform.position.y);
            if (getOffsetVector(gameObject.transform.position).x <= 8.2)
            {
                bossComeUp4th = false;
                attackInProgress = false;
            }

        }

        // Debug.Log("After If Chain Position: " + transform.position.x);
        
        if (!attackInProgress && finishedCutscene)
        {
            timeForAttacks++;
            if (timeForAttacks == (secondsInBetweenAttacks - 1) * 60)
            {
                animator.Play("Snake Tail Rattle");
            }
            if (timeForAttacks <= secondsInBetweenAttacks*60) return;
            timeForAttacks = 0; 
            // Debug.Log("Current Health: " + health.Value);
            // Debug.Log("Attack Counter: " + attack4Counter);
            // Debug.Log("Attack 4 Active?" + snakeAttack4.activeSelf);
            Attack();
            
        }
        
        // Debug.Log("After EVerything Position: " + transform.position.x);
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
            spriteRenderer.color = Color.white;
        });

        // if (health.IsDead)
        // {
        //     spriteRenderer.color = Color.black;
        // }
    }


    public void OnDeath(GameObject attacker)
    {
        // Debug.Log("Snake Boss Defeated!");
        // Debug.Log("Killed by " + attacker.name);
        // Debug.Log("spriteRenderer: " + spriteRenderer == null);
        
        animator.Play("Snake Death");
        //if (spriteRenderer == null) return;
        
        // spriteRenderer.color = Color.black;
        // Replace with scene transition logic
        //UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene"); 
    }

    private int CheckAttack()
    {
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        float playerPositionX;
        if (tempPlayer != null)
        {
            playerPositionX = getOffsetVector(tempPlayer.transform.position).x;
            if (playerPositionX < -(screenSize / 2)) return 3;
            if (playerPositionX < 0) return 2;
            return 1;
        }
        // Debug.Log("no player found");
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
        // Debug.Log("Snake Boss uses Attack 1!");
        attackInProgress = true;
        animator.Play("Snake Tail Whip");
        // Implement attack logic here
    }

    public void callAttack1()
    {
        Attack1Forward = true;
        snakeAttack1.SetActive(true);
    }

    
    public void removeAttack1()
    {
        snakeAttack1.SetActive(false);
        attackInProgress = false;
    }

    private void Attack2()
    {
        // Debug.Log("Snake Boss uses Attack 2!");
        attackInProgress = true;
        bossBackup2nd = true;
        // Implement attack logic here
    }

    public void callAttack2()
    {
        snakeAttack2.SetActive(true);
    }

    public void removeAttack2()
    {
        snakeAttack2.SetActive(false);
    }
    public void BossFinishedAttack2()
    {
        bossComeUp2nd = true;
    }

    private void Attack3()
    {
        // Debug.Log("Snake Boss uses Attack 3!");
        attackInProgress = true;
        animator.Play("Snake Bite Far");
    }

    public void callAttack3()
    {
        snakeAttack3.Attack(this);
    } 

    private void Attack4()
    {
        // Debug.Log("Snake Boss uses its powerful Attack 4!");
        attackInProgress = true;
        bossBackup4th = true;
        // Implement attack logic here
    }

    public void BossFinishedAttack4()
    {
        bossComeUp4th = true;
    }
    
    public void setAttackInProgress(bool b){
        attackInProgress = b;
    }
}
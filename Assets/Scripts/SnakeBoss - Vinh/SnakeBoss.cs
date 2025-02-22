using System;
using System.Collections;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    [SerializeField] private int hitPoints;
    [SerializeField] private int screenSize;
    [SerializeField] private int bodyDamage;
    
    private int attack4Threshold = 5; // Set a default threshold
    private int attack4Counter = 0;

    private int time = 0;

    [SerializeField] private Transform hitBox;

    private int width, length;

    public void Awake()
    {
        //time = Time.fixedTime;
    }

    public void FixedUpdate()
    {
        time++;
        if (time > 200)
        {
            time = 0;
            Debug.Log(time);
            Attack();
        }
    }

private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(bodyDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("you just took " +  damage + " damage");
    }

    public void LoseHitPoints(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Snake Boss Defeated!");
        // Replace with scene transition logic
        UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene"); 
    }

    private int CheckAttack()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        float x;
        if (temp != null)
        {
            x = temp.transform.position.x;
            if (x < -(screenSize / 2))
                return 1;
            else if (x < 0)
                return 2;
            else
                return 3;
        }
        else
        {
            Debug.Log("no player found");
            return 0;
        }
    }

    public int GetSnakeBossBodyDamage()
    {
        return bodyDamage;
    }

    /*
    public bool IsPlayerInRange()
    {
        // Assuming you wanted to check if the player is within attack range
        float distance = Vector3.Distance(saguaro.transform.position, transform.position);
        return distance < 5f; // Example range
    }
    */

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
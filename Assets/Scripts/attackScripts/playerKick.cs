using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


public class playerKickAttack : MonoBehaviour
{
    //Fields for the playerkick attack
    public int enemyDamage = 3;
    public float cooldownTime = 2f;
    public bool isAttack = false;
    private float currentCooldownTime = 0f;

    ///  public Collider hitBox;
    public HashSet<Health> enemies = new HashSet<Health>();


    private void Start()
    {
        //  hitBox.coll
    }

    public void Update()
    {
        //Add a cooldown
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }
        else if (isAttack)
        {
            isAttack = false;
        }
    }

    public void dealDamage()
    {
        if (isAttack == false)
        {
            isAttack = true;
            foreach (Health health in enemies)
            {
                Debug.LogWarning("Hit");
                health.ApplyDamage(enemyDamage, DamageType.PlayerKick, gameObject);
            }

            currentCooldownTime = cooldownTime;
        }
    }

    //check if the hitbox collides with an enemy
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Trigger has been entered.");
        //Apply damage
        if (collision.TryGetComponent<Health>(out Health health))
        {
            enemies.Add(health);
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out Health health))
        {
            enemies.Remove(health);
        }
    }
}
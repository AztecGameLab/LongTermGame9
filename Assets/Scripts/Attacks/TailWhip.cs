using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TailWhip : MonoBehaviour
{
    //Just coppied the front kick for now
    public int enemyDamage = 5;
    public float cooldownTime = 4f;
    public bool isAttack;
    private float currentCooldownTime;

    ///  public Collider hitBox;
    private readonly HashSet<Health> enemies = new();

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

    public void DealDamage(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttack) return;

        isAttack = true;
        currentCooldownTime = cooldownTime;

        foreach (var health in enemies)
        {
            // Debug.LogWarning("Hit");
            health.ApplyDamage(enemyDamage, DamageType.PlayerTailWhip, gameObject);
        }
    }

    //check if the hitbox collides with an enemy
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Trigger has been entered.");
        //Apply damage
        if (collision.TryGetComponent<Health>(out var health))
        {
            enemies.Add(health);
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out var health))
        {
            enemies.Remove(health);
        }
    }
}
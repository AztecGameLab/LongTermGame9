using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FrontKick : MonoBehaviour
{
    private static readonly int Kick = Animator.StringToHash("FrontKick");

    //Fields for the playerkick attack
    public int enemyDamage = 3;
    public float cooldownTime = 2f;
    private bool isAttack;
    private float currentCooldownTime;

    public Animator animator;
    
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
        
        animator.SetTrigger(Kick);
        
        foreach (var health in enemies)
        {
            // Debug.LogWarning("Hit");
            health.ApplyDamage(enemyDamage, DamageType.PlayerKick, gameObject);
        }
    }

    //check if the hitbox collides with an enemy
    public void OnTriggerEnter2D(Collider2D collision)
    {
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
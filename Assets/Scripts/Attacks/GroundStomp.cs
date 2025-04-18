using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class GroundStomp : MonoBehaviour
{
    public int enemyDamage = 1;
    public float coolDownTime = 2f;
    public bool canAttack;
    private float currentCoolDownTime;


    private readonly HashSet<Health> enemies = new();
    // Update is called once per frame
    void Update()
    {
        if(currentCoolDownTime > 0)
        {
            currentCoolDownTime -= Time.deltaTime;
        }
        else if (canAttack)
        {
            canAttack = false;
        }
    }
    public void Stomp(InputAction.CallbackContext context)
    {
        if (canAttack || !context.performed) return;

        canAttack = true;
        currentCoolDownTime = coolDownTime;
        foreach(var health in enemies)
        {
            Debug.Log("Hit");
            health.ApplyDamage(enemyDamage, DamageType.PlayerStomp, gameObject);

        }
    }
    //check if hitbox collides with enemy
    public void OnTriggerEnter2D(Collider2D collision)
    {
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

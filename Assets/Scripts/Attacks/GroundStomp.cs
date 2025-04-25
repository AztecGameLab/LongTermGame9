using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class GroundStomp : MonoBehaviour
{
    private static readonly int GroundStomping = Animator.StringToHash("GroundStomping");
    
    public int enemyDamage = 1;
    public float coolDownTime = 2f;
    public bool canAttack;
    private float currentCoolDownTime;
    private float audioInterval;
    
    [SerializeField] private AudioSource stompSound = null;
    [SerializeField] private PlayerMovementControl controller;
    [SerializeField] private Animator animator;
    
    private readonly HashSet<Health> enemies = new();

    // Update is called once per frame

    public bool DoStomp { get; private set; }
    
    void Update()
    {
        if(currentCoolDownTime > 0){ currentCoolDownTime -= Time.deltaTime; }
        else if (canAttack){ canAttack = false; }
        
        if (currentCoolDownTime < coolDownTime / 2)
        {
            DoStomp = false;
            animator.SetBool(GroundStomping, false);
        }
        else if (currentCoolDownTime < audioInterval - 0.5f && DoStomp)
        {
            stompSound.Play();
            audioInterval -= 0.5f;
        }
        
        
    }
    public void Stomp(InputAction.CallbackContext context)
    {
        if (canAttack || !context.performed || !controller.IsGrounded())
        {
            DoStomp = false;
            if (currentCoolDownTime > coolDownTime / 2)
            {
                currentCoolDownTime = coolDownTime / 2;
            }
            animator.SetBool(GroundStomping, false);
            controller.AllowMovement = true;
            return;
        }
        animator.SetBool(GroundStomping, true);
        stompSound.Play();
        controller.AllowMovement = false;
        canAttack = true;
        currentCoolDownTime = coolDownTime;
        audioInterval = coolDownTime;
        DoStomp = true;
        
        foreach(var health in enemies)
        {
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

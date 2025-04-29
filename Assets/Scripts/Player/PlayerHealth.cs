using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void onDamage(int damage, DamageType damageType, GameObject attacker)
    {
        
    }

    public void onDeath(GameObject attacker)
    {
        Debug.Log("Killed by " + attacker.name);
    }
}

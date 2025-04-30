using Player;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Scene] public string deathScene;
    public PlayerSfx playerSfx;
    
    public void onDamage(int damage, DamageType damageType, GameObject attacker)
    {
        if (damage > 0)
        {
            playerSfx.PlayHitSfx();
        }
    }

    public void onDeath(GameObject attacker)
    {
        SceneManager.LoadScene(deathScene);
    }
}

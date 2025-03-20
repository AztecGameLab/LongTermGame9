using UnityEngine;

public class AttackExample : MonoBehaviour
{
    public GameObject projectilePrefab;

    private void Start()
    {
        InvokeRepeating(nameof(Attack), 2, 2);
    }

    public void Attack()
    {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        if (projectile.TryGetComponent<Rigidbody2D>(out var projectileRb))
        {
            projectileRb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }
    }
    
}

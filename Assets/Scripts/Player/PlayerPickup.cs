using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private AmmoManager ammoManager;
    [SerializeField] private Health health;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<CactusSpine>(out var spine))
        {
            ammoManager.IncreaseAmmo(1);
            Destroy(spine.gameObject);
        }

        if (other.gameObject.TryGetComponent<BushSeed>(out var seed))
        {
            health.AddHealth(1);
            Destroy(seed.gameObject);
        }
    }
}

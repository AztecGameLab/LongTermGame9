using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private AmmoManager ammoManager;
    [SerializeField] private Health health;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<CactusSpine>(out var spine))
        {
            ammoManager.IncreaseAmmo(1);
        }

        if (other.TryGetComponent<BushSeed>(out var seed))
        {
            health.AddHealth(1);
        }
    }
}

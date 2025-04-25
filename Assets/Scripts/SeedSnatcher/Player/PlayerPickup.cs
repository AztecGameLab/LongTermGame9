using CactusBushes;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private AmmoManager ammoManager;
    [SerializeField] private Health health;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<DroppableItem>(out var itemPickup))
        {
            itemPickup.OnCollect(health, ammoManager);
            Destroy(itemPickup.gameObject);
        }
    }
}

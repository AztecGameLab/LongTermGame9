using UnityEngine;

namespace CactusBushes
{
    public class CactusSpine : DroppableItem
    {
        [SerializeField] private int ammoReward = 1;
        
        public override void OnCollect(Health health, AmmoManager ammoManager)
        {
               ammoManager.IncreaseAmmo(ammoReward);
        }
    }
}

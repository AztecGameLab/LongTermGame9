using UnityEngine;

namespace CactusBushes
{
    public abstract class DroppableItem : MonoBehaviour
    {
        public abstract void OnCollect(Health health, AmmoManager ammoManager); // should generalize to a player stats object
    }
}
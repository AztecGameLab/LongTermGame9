using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

namespace SeedSnatcher.Seed
{
    public class SeedManager : MonoBehaviour
    {
            private static SeedManager instance;
            private List<SnatchableSeed> unexpiredSeeds = new();
            private List<SnatchableSeed> expiredSeeds = new();

            public SeedManager GetInstance()
            {
                if (instance.IsUnityNull()) instance = this;
                return instance;
            }
            
            public void AddSeed(SnatchableSeed snatchableSeed)
            {
                if (snatchableSeed.isExpired)
                {
                    expiredSeeds.Add(snatchableSeed);                    
                }
                else
                {
                    unexpiredSeeds.Add(snatchableSeed);
                }
            }

            public void RemoveSeed(SnatchableSeed snatchableSeed)
            {
                expiredSeeds.Remove(snatchableSeed);
            }

            private void CheckExpiredSeeds()
            {
                var newlyExpiredSeeds = unexpiredSeeds.Where(seed => seed.isExpired).ToList();
                expiredSeeds = expiredSeeds.Concat(newlyExpiredSeeds).ToList();
                unexpiredSeeds = unexpiredSeeds.Except(newlyExpiredSeeds).ToList();
            }
            
            private void Update()
            {
                CheckExpiredSeeds();
            }

            public SnatchableSeed GetNearestSeed(Vector3 position)
            {
                SnatchableSeed closestSeed = null;
                var shortestDistance = float.MaxValue;
                foreach (var seed in expiredSeeds)
                {
                    var distance = Vector3.Distance(position, seed.transform.position);
                    if (distance < shortestDistance)
                    {
                        closestSeed = seed;
                        shortestDistance = distance;
                    }
                }

                return closestSeed;
            }
            

    }
}
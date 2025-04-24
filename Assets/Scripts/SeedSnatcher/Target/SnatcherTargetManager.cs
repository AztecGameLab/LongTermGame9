using System.Collections.Generic;
using System.Linq;
using FMOD;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedSnatcher.Target
{
    public class SnatcherTargetManager : MonoBehaviour
    {
            private static SnatcherTargetManager instance;
            private List<SnatchableTarget> unexpiredSeeds = new();
            private List<SnatchableTarget> expiredSeeds = new();

            public SnatcherTargetManager GetInstance()
            {
                if (instance.IsUnityNull()) instance = this;
                return instance;
            }
            
            public void AddSeed(SnatchableTarget snatchableTarget)
            {
                if (snatchableTarget.isExpired)
                {
                    expiredSeeds.Add(snatchableTarget);                    
                }
                else
                {
                    unexpiredSeeds.Add(snatchableTarget);
                }
            }

            public void RemoveSeed(SnatchableTarget snatchableTarget)
            {
                
                if (snatchableTarget.isExpired)
                {
                    expiredSeeds.Remove(snatchableTarget);
                }
                else
                {
                    unexpiredSeeds.Remove(snatchableTarget);
                }
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

            public SnatchableTarget GetNearestSeed(Vector3 position, float maximumRange)
            {
                SnatchableTarget closestTarget = null;
                var shortestDistance = float.MaxValue;
                foreach (var seed in expiredSeeds)
                {
                    if (seed.isBeingTargeted)
                    {
                        continue;
                    }

                    var distance = Vector3.Distance(position, seed.transform.position);

                    if (distance > maximumRange)
                    {
                        continue;
                    }

                    if (distance < shortestDistance)
                    {
                        closestTarget = seed;
                        shortestDistance = distance;
                    }
                }

                return closestTarget;
            }
            

    }
}
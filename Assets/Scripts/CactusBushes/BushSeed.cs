using System;
using Cysharp.Threading.Tasks;
using Player.Throwables;
using SeedSnatcher.Target;
using Unity.VisualScripting;
using UnityEngine;

namespace CactusBushes
{
    public class BushSeed : DroppableItem
    {
            [SerializeField] private int healthReward = 1;
            public bool isStopped;
            private const float StoppingThreshold = 1f;
            private Rigidbody2D rb;
            private CircleCollider2D sc;
            private SnatchableTarget targetMarker;
            private const float GracePeriod = 1.0f; // prevent seed from freezing on spawn
            private float graceTime = GracePeriod;
            private bool slowDown;
        
            public override void OnCollect(Health health, AmmoManager ammoManager)
            {
                health.AddHealth(healthReward);
            }

            private async void Start()
            {
                rb = GetComponent<Rigidbody2D>();
                targetMarker = GetComponent<SnatchableTarget>();
                sc = GetComponent<CircleCollider2D>();
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                if(sc){sc.excludeLayers &= ~(1 << LayerMask.NameToLayer("Player"));}
                slowDown = true;
                
            }

            private void Update()
            {
                var speed = rb.linearVelocity.magnitude;
                var belowStopThreshold = speed < StoppingThreshold;
                
                if (belowStopThreshold)
                {
                    graceTime -= Time.deltaTime;

                    if (graceTime <= 0)
                    { 
                        isStopped = true;
                        rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
                else
                {
                    graceTime = GracePeriod;
                    if (slowDown)
                    {
                        rb.linearVelocity *= 0.4f;
                    }
                }
                
                targetMarker.isStopped = isStopped;
            }

    }
}

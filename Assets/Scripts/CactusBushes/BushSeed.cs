using SeedSnatcher.Target;
using UnityEngine;

namespace CactusBushes
{
    public class BushSeed : DroppableItem
    {
            [SerializeField] private int healthReward = 1;
            private bool isStopped;
            private const float StoppingThreshold = 0.1f;
            private Rigidbody2D rb;
            private SnatchableTarget targetMarker;
            private const float GracePeriod = 1.0f; // prevent seed from freezing on spawn
            private float graceTime = GracePeriod;
        
            public override void OnCollect(Health health, AmmoManager ammoManager)
            {
                health.AddHealth(healthReward);
            }

            private void Start()
            {
                rb = GetComponent<Rigidbody2D>();
                targetMarker = GetComponent<SnatchableTarget>();
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
                }
                
                targetMarker.canExpire = isStopped;
            }

    }
}

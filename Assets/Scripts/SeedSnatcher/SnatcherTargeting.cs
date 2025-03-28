using SeedSnatcher.Seed;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedSnatcher
{
    public class SnatcherTargeting : MonoBehaviour
    {
        [SerializeField] private SnatchableSeed target;
        [SerializeField] private bool doFindTarget = true;
        
        private SeedManager seedManager;

        private void Start()
        {
            seedManager = FindFirstObjectByType<SeedManager>().GetInstance();
        }

        private void SetTarget(SnatchableSeed newTarget)
        {
            target = newTarget;
        }

        // Find seed GameObject that have been on the ground too long 
        public void FindTarget()
        {
            if (!doFindTarget)
            {
                return;
            }
            
            var thisPosition = transform.position;
            var possibleTarget = seedManager.GetNearestSeed(thisPosition);
            if (!possibleTarget.IsUnityNull()) {
                SetTarget(possibleTarget);
            }
        }

        public bool HasTarget()
        {
            return !target.IsUnityNull();
        }

        public void DestroyTarget()
        {         
            target.Destroy();
            target = null;
        }

        public Vector3 GetTargetPosition()
        {
            return target.transform.position;
        }
    }
}
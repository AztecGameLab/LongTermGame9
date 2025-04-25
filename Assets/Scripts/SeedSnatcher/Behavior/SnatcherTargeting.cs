using SeedSnatcher.Target;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedSnatcher.Behavior
{
    public class SnatcherTargeting : MonoBehaviour
    {
        [SerializeField] private SnatchableTarget target;
        [SerializeField] private bool doFindTarget = true;
        public float maximumRange = 100f;
        
        private SnatcherTargetManager seedManager;

        private void Start()
        {
            seedManager = FindFirstObjectByType<SnatcherTargetManager>().GetInstance();
        }

        private void SetTarget(SnatchableTarget newTarget)
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
            var possibleTarget = seedManager.GetNearestSeed(thisPosition, maximumRange);
            if (!possibleTarget.IsUnityNull()) {
                possibleTarget.isBeingTargeted = true;
                SetTarget(possibleTarget);
            }
        }

        public bool HasTarget()
        {
            var isNull = target.IsUnityNull();
            if (isNull)
            {
                target = null;
            }
            return !isNull;
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
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

        private SnatchableTarget FindNearestTarget()
        {
            var thisPosition = transform.position;
            var possibleTarget = seedManager.GetNearestSeed(thisPosition, maximumRange);
            return !possibleTarget.IsUnityNull() ? possibleTarget : null;
        }
        
        // Find seed GameObject that have been on the ground too long 
        public void FindTarget()
        {
            if (!doFindTarget)
            {
                return;
            }
            var newTarget = FindNearestTarget();
            SetTarget(newTarget);
        }

        public void ReevaluateTarget()
        {
            var newTarget = FindNearestTarget();
            if (newTarget != target)
            {
                SetTarget(newTarget);
            }
        }

        public bool HasTarget()
        {
            var isNull = target.IsUnityNull();
            if (isNull)
            {
                target = null;
                return false;
            }
            var isBeingPursued = !target.pursuant.IsUnityNull() && target.pursuant != gameObject;
            if (isBeingPursued)
            {
                target = null;
                return false;
            }
            return true;
        }

        public void ClaimTarget()
        {
            target.pursuant = gameObject;
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
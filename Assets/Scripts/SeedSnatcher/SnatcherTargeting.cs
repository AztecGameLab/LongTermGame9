using Unity.VisualScripting;
using UnityEngine;

namespace SeedSnatcher
{
    public class SnatcherTargeting : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private string targetTag = "ExpiredSeed";
        [SerializeField] private bool doFindTarget = true;

        private void SetTarget(GameObject newTarget)
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
            
            var possibleTarget = GameObject.FindWithTag(targetTag);
            // TODO: check whether already targeted by another snatcher
            if (!possibleTarget.IsUnityNull())
            {
                SetTarget(possibleTarget);
            }
        }

        public bool HasTarget()
        {
            return !target.IsUnityNull();
        }

        public void RemoveTarget()
        {
            target = null;
        }

        public Vector3 GetTargetPosition()
        {
            return target.transform.position;
        }

        
    }
}
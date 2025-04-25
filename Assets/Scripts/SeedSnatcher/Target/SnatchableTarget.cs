using UnityEngine;

namespace SeedSnatcher.Target
{
    public class SnatchableTarget : MonoBehaviour
    {
        /**
         * "Expire" refers to when the seed has
         * been on the ground for too long.
         */
        [SerializeField] private float expiryTime = 10.0f;
        public bool canExpire = true;
        private float timer;
        public bool isExpired;
        public bool isBeingTargeted;

        private SnatcherTargetManager seedManager;

        private void Start()
        {
            seedManager = FindFirstObjectByType<SnatcherTargetManager>().GetInstance();
            seedManager.AddSeed(this);
        }
        
        private void Update()
        {
            if (!canExpire) return;
            
            timer += Time.deltaTime;

            if (timer >= expiryTime)
            {
                isExpired = true;
            }
        }

        public void OnDestroy()
        {
            seedManager.RemoveSeed(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
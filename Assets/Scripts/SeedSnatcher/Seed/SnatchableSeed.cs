using UnityEngine;

namespace SeedSnatcher.Seed
{
    public class SnatchableSeed : MonoBehaviour
    {
        /**
         * "Expire" refers to when the seed has
         * been on the ground for too long.
         */
        [SerializeField] private float expiryTime = 10.0f;
        [SerializeField] private bool canExpire = true;
        private float timer;
        public bool isExpired;
        public bool isBeingTargeted;

        private SeedManager seedManager;

        private void Start()
        {
            seedManager = FindFirstObjectByType<SeedManager>().GetInstance();
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

        public void Destroy()
        {
            seedManager.RemoveSeed(this);
            Destroy(gameObject);
        }
    }
}
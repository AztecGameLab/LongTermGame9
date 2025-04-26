using CactusBushes;
using Player.Throwables;
using UnityEngine;

namespace Player
{
    public class PlayerPickup : MonoBehaviour
    {
        [SerializeField] private AmmoManager ammoManager;
        [SerializeField] private Health health;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        private void PlayAudio()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<DroppableItem>(out var itemPickup))
            {
                PlayAudio();
                itemPickup.OnCollect(health, ammoManager);
                Destroy(itemPickup.gameObject);
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace SeedSnatcher
{
    public class SnatcherSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip snatcherDiveSfx;
        [SerializeField] private AudioClip snatcherDestroySfx;

        public void PlayBeginDive()
        {
            audioSource.clip = snatcherDiveSfx;
            audioSource.Play();
        }

        public void PlayDestroy()
        {
            audioSource.PlayOneShot(snatcherDestroySfx);
        }
    }
}
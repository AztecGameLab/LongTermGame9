using UnityEngine;

namespace SeedSnatcher
{
    public class SnatcherSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip snatcherDiveSfx;
        
        public void PlayBeginDive()
        {  
            PlaySfx(snatcherDiveSfx);
        }

        private void PlaySfx(AudioClip newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
        
    }
}
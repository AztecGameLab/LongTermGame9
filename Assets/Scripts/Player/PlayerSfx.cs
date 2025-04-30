using System;
using UnityEngine;

namespace Player
{
    public class PlayerSfx : MonoBehaviour
    {
        [SerializeField] private PlayerMovementControl movementControl;

        [Header("Movement")] [Tooltip("For when the character travels along the ground.")] [SerializeField]
        private AudioSource walkAudioSource;

        [Tooltip("For when the character jumps off the ground.")] [SerializeField]
        private AudioSource jumpAudioSource;

        [SerializeField] private AudioClip[] jumpAudioClips;

        [Tooltip("For when the character jumps a second time in the air.")] 
        [SerializeField] private AudioClip[] doubleJumpAudioClips;

        [Tooltip("For when the character hits the ground.")] 
        [SerializeField] private AudioClip[] landingAudioClips;
        
        [Tooltip("For when the character is attacked.")]
        [SerializeField] private AudioClip[] hitAudioClips;


        [Header("Thresholds")]
        [Tooltip("How long after leaving the ground should the walk sound cut off?")]
        [SerializeField]
        private float walkAirTimeThreshold = 0.2f;

        [Tooltip("How long after leaving the ground should the landing sound be able to play?")] [SerializeField]
        private float landingAirTimeThreshold = 0.8f;


        private float airTime;

        private bool IsInAir => airTime > walkAirTimeThreshold;

        private bool wasInAir;

        public void PlayJumpSfx()
        {
            jumpAudioSource.PlayOneShot(jumpAudioClips[UnityEngine.Random.Range(0, jumpAudioClips.Length)]);
        }

        public void PlayDoubleJumpSfx()
        {
            jumpAudioSource.PlayOneShot(doubleJumpAudioClips[UnityEngine.Random.Range(0, doubleJumpAudioClips.Length)]);
        }

        private void PlayLandingSfx()
        {
            jumpAudioSource.PlayOneShot(landingAudioClips[UnityEngine.Random.Range(0, landingAudioClips.Length)], 0.6f);
        }
        
        public void PlayHitSfx()
        {
            jumpAudioSource.PlayOneShot(hitAudioClips[UnityEngine.Random.Range(0, hitAudioClips.Length)], 0.6f);
        }

        private void HandleWalkSfx()
        {
            if (movementControl.IsMoving && !walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
            }
            else if (!movementControl.IsMoving)
            {
                walkAudioSource.Pause();
            }
        
            if (IsInAir)
            {
                walkAudioSource.Pause();
            }
        }

        private void Update()
        {
            if (movementControl.IsGrounded())
            {
                if (wasInAir)
                {
                    wasInAir = false;
                    PlayLandingSfx();
                }
            
                airTime = 0.0f;
            }
            else
            {
                airTime += Time.deltaTime;
                if (airTime > landingAirTimeThreshold) wasInAir = true;
            }
            
            HandleWalkSfx();
        }
    }
}
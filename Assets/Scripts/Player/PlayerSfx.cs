using System;
using UnityEngine;

namespace Player
{
    public class PlayerSfx : MonoBehaviour
    {
        [SerializeField] private PlayerMovementControl movementControl;

        [Header("Movement")]
        [Tooltip("For when the character travels along the ground.")]
        [SerializeField] private AudioSource walkAudioSource;
        [Tooltip("For when the character jumps off the ground.")]
    
        [SerializeField] private AudioSource jumpAudioSource;
        [Tooltip("For when the character hits the ground.")]
        [SerializeField] private AudioSource landingAudioSource;

        [Header("Thresholds")]        
        [Tooltip("How long after leaving the ground should the walk sound cut off?")]
        [SerializeField] private float walkAirTimeThreshold = 0.2f;
        [Tooltip("How long after leaving the ground should the landing sound be able to play?")]
        [SerializeField] private float landingAirTimeThreshold = 0.8f;


        private float AirTime;
        private bool isInAir { get {
            return AirTime > walkAirTimeThreshold;
        } }
        private bool wasInAir;

        private void HandleWalkSFX()
        {
            if (movementControl.IsMoving && !walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
            }
            else if (!movementControl.IsMoving)
            {
                walkAudioSource.Stop();
            }
            if (isInAir)
            {
                walkAudioSource.Stop();
            }
        }

        private void Update()
        {
            if (movementControl.IsGrounded())
            {
                if (wasInAir)
                {
                    wasInAir = false;
                    landingAudioSource.Play();
                }
                AirTime = 0.0f;
            }
            else
            {
                AirTime += Time.deltaTime;
                if (AirTime > landingAirTimeThreshold) wasInAir = true;
            }

            HandleWalkSFX();
        }

        private void OnEnable()
        {
            movementControl.OnJumped += HandleJumpSFX;
        }

        private void OnDisable()
        {
            movementControl.OnJumped -= HandleJumpSFX;
        }

        private void HandleJumpSFX()
        {
            jumpAudioSource.Play();
        }
    }

}
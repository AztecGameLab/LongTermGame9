using System.Collections.Generic;
using SeedSnatcher.Movement;
using UnityEngine;

namespace SeedSnatcher
{
    public enum SnatcherState
    {
        Idle,
        Diving
    }
    
    public class SnatcherController : MonoBehaviour
    {
        private SnatcherMovement snatcherMovement;
        private Dictionary<SnatcherState, SnatcherMovement> snatcherMovements;

        public void SetState(SnatcherState newState)
        {
            snatcherMovements.TryGetValue(newState, out snatcherMovement);
            snatcherMovement?.Init();
        }

        private void Start()
        {
            snatcherMovements = new Dictionary<SnatcherState, SnatcherMovement>()
            {
                { SnatcherState.Idle, GetComponent<SnatcherIdle>()},
                { SnatcherState.Diving, GetComponent<SnatcherDive>()}
            };
            
            SetState(SnatcherState.Idle);
        }

        private void Update()
        {
            snatcherMovement.Loop();
        }
    }
}
using System.Collections.Generic;
using SeedSnatcher.Behavior.Movement;
using UnityEngine;

namespace SeedSnatcher
{
    public enum SnatcherState
    {
        Idle,
        Diving,
        Investigate
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
                { SnatcherState.Diving, GetComponent<SnatcherDive>()},
                { SnatcherState.Investigate, GetComponent<SnatcherInvestigate>()}
            };
            
            SetState(SnatcherState.Idle);
        }

        private void Update()
        {
            snatcherMovement.Loop();
        }
    }
}
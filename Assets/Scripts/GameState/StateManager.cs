using UnityEngine;
using System.Collections.Generic;

namespace GameState
{
    public class StateManager : MonoBehaviour
    {
        private State currentState;
            
        private readonly Dictionary<State, IGameState> stateMapping = new(){
            { State.MainMenu, null },
            { State.PauseMenu, null },
            { State.Combat, null },
            { State.Exploration, new ExplorationState() },
            { State.GameOver, null }
        };

        void ChangeState(State state)
        {
            if (stateMapping[currentState].Teardown())
            {
                if (stateMapping[state].Setup())
                {
                    currentState = state;
                    stateMapping[state].Run();
                }
                else
                {
                    Debug.LogError("failed to switch to state " + state);
                }
            }
            else
            {
                Debug.LogError("failed to teardown state " + currentState);
            }
        }
    }   
}

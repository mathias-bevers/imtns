using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerMiniGameState : GameState
    {
        private void OnGUI()
        {
            if (!IsActive)
            {
                return;
            }

            if (GUI.Button(new Rect(10, 10, 300, 200), GetType().Name + ": Return to roaming state"))
            { 
                GameStateController.Instance.SwitchToState<RoamingGameState>();
            }
        }
    }
}
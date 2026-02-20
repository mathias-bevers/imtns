using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class ExampleAState : GameState
    {
        private void OnGUI()
        {
            if (!isActive)
            {
                return;
            }

            if (GUI.Button(new Rect(10, 10, 300, 200), GetType().Name + ": Return to roaming state"))
            {
                GameStateController.instance.SwitchToState<RoamingGameState>();
            }
        }
    }
}
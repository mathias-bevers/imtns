using CleanRoom.Movement;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class RoamingGameState : GameState
    {
        [SerializeField] private CameraFollow follow;

        protected override void OnEnter()
        {
            follow.shouldFollow = true;
        }

        protected override void OnExit()
        {
            follow.shouldFollow = false;
        }
    }
}
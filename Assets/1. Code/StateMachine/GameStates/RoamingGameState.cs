using CleanRoom.Movement;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class RoamingGameState : GameState
    {
        [SerializeField] private CameraFollow follow;
        
        public override void OnEnter()
        {
            base.OnEnter();
            follow.shouldFollow = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            follow.shouldFollow = false;
        }
    }
}
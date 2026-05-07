using CleanRoom.Movement;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerRoomState : State
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
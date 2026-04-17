using System;
using UnityEngine;

namespace CleanRoom.ScriptableStateMachine
{
    [CreateAssetMenu(fileName = "RoomState", menuName = "CleanRoom/RoomState", order = 0)]
    public class ScriptableRoomState : ScriptableState
    {
        [field: SerializeField] public ScriptableRoomState NextRoom { get; private set; }

        public void GotoNextRoom()
        {
            bool succes = StateMachine.Instance.TryEnterState(NextRoom);
            
            if (succes)
            {
                return;
            }

            throw new Exception($"could switch to state: {NextRoom.name}");
        }
    }
}
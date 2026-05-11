using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.StateMachine
{
    public class RoomState : State
    {
        [field: SerializeField] public RoomState NextRoom { get; private set; }
        [field: SerializeField] public UnityEvent UnfocusEvent { get; private set; }
    }
}
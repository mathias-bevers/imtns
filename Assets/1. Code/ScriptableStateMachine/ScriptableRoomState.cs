using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.ScriptableStateMachine
{
    [CreateAssetMenu(fileName = "RoomState", menuName = "CleanRoom/RoomState", order = 0)]
    public class ScriptableRoomState : ScriptableState
    {
        [field: SerializeField] public ScriptableRoomState NextRoom { get; private set; }
    }
}
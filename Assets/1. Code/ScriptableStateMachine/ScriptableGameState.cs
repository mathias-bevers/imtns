using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.ScriptableStateMachine
{
    [CreateAssetMenu(fileName = "GameState", menuName = "CleanRoom/GameState", order = 0)]
    public class ScriptableGameState : ScriptableState
    {
        public int Attempts { get; private set; } = 0;
    }
}
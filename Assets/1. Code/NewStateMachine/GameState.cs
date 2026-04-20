using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.NewStateMachine
{
    public class GameState : State
    {
        public int Attempts { get; private set; } = 0;
    }
}
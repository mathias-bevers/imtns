using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class StateSwitchEvent : MonoBehaviour
    {
        private static DropdownList<GameState> _gameStateValues = null;

        [Dropdown("GetGameStateValues"), SerializeField] private GameState state;

        public void SwitchToState()
        {
            GameStateController.Instance.SwitchToState(state);
        }

        private static DropdownList<GameState> GetGameStateValues()
        {
            if (!ReferenceEquals(null, _gameStateValues))
            {
                return _gameStateValues;
            }

            GameState[] gameStates = FindObjectsByType<Component>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .OfType<GameState>().ToArray();

            _gameStateValues = new DropdownList<GameState>();
            for (int i = 0; i < gameStates.Length; ++i)
            {
                GameState gameState = gameStates[i];
                _gameStateValues.Add(gameState.name, gameState);
            }

            return _gameStateValues;
        }
    }
}
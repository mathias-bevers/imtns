using System.Linq;
using CleanRoom.Menus;
using CleanRoom.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.StateMachine
{
    public class RoomState : State
    {
        [field: SerializeField] public RoomState NextRoom { get; private set; }
        [field: SerializeField] public UnityEvent UnfocusEvent { get; private set; }
        [SerializeField] private GameState[] gameStates;

        public override bool CanExit => StateMachine.Instance.IsStateCompleted(StateName);

        public override void Enter()
        {
            StateMachine stateMachine = StateMachine.Instance;
            bool completedAllGames =
                gameStates.All(gameState => stateMachine.IsStateCompleted(gameState.GetType().Name));

            if (gameStates.IsNullOrEmpty())
            {
                Complete();
            }
            else if (completedAllGames)
            {
                MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(
                    "Je hebt alle minigames in deze kamer af gerond, je kan naar de volgende",
                    Popup.MessageType.Correct, StateName + " is voltooit!");
                Complete();
            }
            
            base.Enter();

        }

        public string[] GetGameStateNames() => gameStates.Select(gameState => gameState.GetType().Name).ToArray();
    }
}
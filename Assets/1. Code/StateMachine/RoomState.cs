using System;
using System.Linq;

namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     The class is the base of all RoomStates
    /// </summary>
    public abstract class RoomState : State
    {
        /// <summary>
        ///     All the games that are part of this room. Set in the
        ///     <see cref="Initialize" /> method
        /// </summary>
        private GameState[] gameStates;


        public override void Initialize()
        {
            base.Initialize();
            //TODO: fill game states
            gameStates = Array.Empty<GameState>();
        }

        /// <summary>
        ///     This method indicates whether a <see cref="RoomState" /> can be entered.
        /// </summary>
        /// <returns><see langword="true" /> if room is not yet completed.</returns>
        public virtual bool CanEnter() => !IsCompleted;

        /// <summary>
        ///     This method indicates wheter a <see cref="RoomState" /> can be exited.
        /// </summary>
        /// <returns><see langword="true" /> all rooms are completed.</returns>
        public virtual bool CanExit()
        {
            for (int i = 0; i < gameStates.Length; ++i)
            {
                if (gameStates[i].IsCompleted)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get the <see cref="GameStateData" /> for each <see cref="GameState" /> in
        ///     the <see cref="RoomState" />.
        /// </summary>
        /// <returns>An array of <see cref="GameStateData" />.</returns>
        public GameStateData[] GetRoomResults() =>
            gameStates.Select(gameState => gameState.GetAsData()).ToArray();
    }
}
namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     This class is the base for all the <see cref="State" /> that hold a game.
    /// </summary>
    public abstract class GameState : State
    {
        /// <summary>
        ///     Times the user has attempted to complete the game.
        /// </summary>
        public int Attempt { get; private set; } = 0;

        /// <summary>
        ///     When the room enters, the <see cref="Attempt" /> property is increased by one.
        /// </summary>
        protected override void OnEnter() => ++Attempt;

        /// <summary>
        ///     Unless the game is completed, the game is reset on exit.
        /// </summary>
        protected override void OnExit()
        {
            if (IsCompleted)
            {
                return;
            }

            Reset();
        }

        /// <summary>
        ///     Resets the game to the starting state.
        /// </summary>
        protected abstract void Reset();
    }
}
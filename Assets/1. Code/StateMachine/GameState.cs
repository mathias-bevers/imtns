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

        public abstract string Name { get; }

        /// <summary>
        ///     When the room enters, the <see cref="Attempt" /> property is increased by
        ///     one.
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

        /// <summary>
        ///     Converts the <see cref="GameState" /> into <see cref="GameStateData" />.
        /// </summary>
        /// <returns>
        ///     <see cref="GameStateData" /> of the current <see cref="GameState" />.
        /// </returns>
        public GameStateData GetAsData() => new(Name, Attempt, IsCompleted);
    }

    /// <summary>
    ///     A struct to that captures the essential data of the <see cref="GameState" /> for
    ///     displaying info in the hud.
    /// </summary>
    public struct GameStateData
    {
        public readonly string name;
        public readonly int attempts;
        public readonly bool completed;

        public GameStateData(string name, int attempts, bool completed)
        {
            this.name = name;
            this.attempts = attempts;
            this.completed = completed;
        }
    }
}
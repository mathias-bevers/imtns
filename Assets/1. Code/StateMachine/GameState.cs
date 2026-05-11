namespace CleanRoom.StateMachine
{
    public class GameState : State
    {
        public int Attempts { get; private set; } = 0;
    }
}
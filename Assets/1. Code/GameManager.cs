namespace CleanRoom
{
    public class GameManager : Singleton<GameManager>
    {
        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        public void OnMistakeMade(string stateName, string message)
        {
            //TODO: call popup
            FeedbackLogger.AddFeedback(stateName, message);
        }
    }
}
using CleanRoom.Menus;

namespace CleanRoom
{
    public class GameManager : Singleton<GameManager>
    {
        private PopupMenu popupMenu = null;
        
        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();

            popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();
        }

        public void OnMistakeMade(string stateName, string message)
        {
            popupMenu.CreatePopup(message, Popup.MessageType.Incorrect);
            FeedbackLogger.AddFeedback(stateName, message);
        }
    }
}
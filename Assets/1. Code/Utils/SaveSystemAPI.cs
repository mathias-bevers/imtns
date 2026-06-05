using UnityEngine;

namespace CleanRoom.Utils
{
    [CreateAssetMenu(fileName = "Save System API", menuName = "CleanRoom/Save System API")]
    public class SaveSystemAPI : ScriptableObject
    {
        public void ResetSaves()
        {
            SaveSystem.Reset();
            FeedbackLogger.Reset();
        }
    }
}
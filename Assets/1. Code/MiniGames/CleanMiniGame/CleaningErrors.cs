using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    [CreateAssetMenu(fileName = "Cleaning Step Error", menuName = "CleanRoom/Cleaning Step Error")]
    public class CleaningErrors : ScriptableObject
    {
        [field: SerializeField] public Tablet.CleanlinessLevel ExpectedLevel { get; private set; }
        [field: SerializeField, TextArea] public string AlreadyCompletedMessage { get; private set; }
        [field: SerializeField, TextArea] public string NotThereMessage { get; private set; }
    }
}
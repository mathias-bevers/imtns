using System;
using CleanRoom.Menus;
using UnityEngine;

namespace CleanRoom.Utils
{
    [CreateAssetMenu(fileName = "Clean Room API", menuName = "CleanRoom/API")]
    public class CleanRoomAPI : ScriptableObject
    {
        public void ResetSaves()
        {
            SaveSystem.Reset();
            FeedbackLogger.Reset();
        }
    }
}
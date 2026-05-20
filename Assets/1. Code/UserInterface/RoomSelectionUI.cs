using System;
using CleanRoom.StateMachine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class RoomSelectionUI : MonoBehaviour
    {
        [SerializeField] private TransformStatePair<RoomState>[] transformStatePairs;

        private void OnEnable()
        {
            JObject mistakes = SaveSystem.GetGameMistakes();
            Debug.Log(mistakes.ToString());

            for (int i = 0; i < transformStatePairs.Length; ++i)
            {
                TransformStatePair<RoomState> transformStatePair = transformStatePairs[i];
                TextMeshProUGUI text = transformStatePair.Transform.GetComponentInChildren<TextMeshProUGUI>();

                if (ReferenceEquals(null, transformStatePair.State))
                {
                    text.SetText("Nog niet hier!");
                    continue;
                }

                string[] gameStateNames = transformStatePair.State.GetGameStateNames();

                for (int ii = 0; ii < gameStateNames.Length; ++ii)
                {
                    Debug.Log(mistakes[gameStateNames[i]]);
                }
            }
        }


        [Serializable]
        public struct TransformStatePair<T> where T : State
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public T State { get; private set; }
        }
    }
}
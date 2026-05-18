using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class GameState : State
    {
        private static readonly TimeSpan MISTAKE_COOLDOWN = new(0, 0, 1);

        private static JObject _gameMistakes;

        private DateTime previousMistakeTime;
        private int mistakes = 0;


        public override void Enter()
        {
            _gameMistakes = SaveSystem.GetGameMistakes();
            JToken token = _gameMistakes[GetType().Name];

            if (ReferenceEquals(null, token))
            {
                throw new KeyNotFoundException($"could not find jToken for key: {GetType().Name}");
            }

            mistakes = token.ToObject<int>();
            base.Enter();
        }

        protected void OnMistakeMade()
        {
            if (DateTime.Now - previousMistakeTime < MISTAKE_COOLDOWN)
            {
                return;
            }

            ++mistakes;
            _gameMistakes[GetType().Name] = mistakes;
            SaveSystem.SaveFile(SaveSystem.GAME_MISTAKES, _gameMistakes.ToString());
            Debug.Log(SaveSystem.LoadFile(SaveSystem.GAME_MISTAKES));
            previousMistakeTime = DateTime.Now;
        }
    }
}
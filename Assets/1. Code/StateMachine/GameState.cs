using System;
using System.Collections.Generic;
using CleanRoom.Menus;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class GameState : State
    {
        private static readonly TimeSpan MISTAKE_COOLDOWN = new(0, 0, 1);
        private static JObject _gameMistakes;

        private DateTime previousMistakeTime;

        public override void Enter()
        {
            _gameMistakes = SaveSystem.GetGameMistakes();
            JToken token = _gameMistakes[GetType().Name];

            if (ReferenceEquals(null, token))
            {
                throw new KeyNotFoundException($"could not find jToken for key: {GetType().Name}");
            }

            base.Enter();
        }

        protected void OnMistakeMade(string message)
        {
            if (DateTime.Now - previousMistakeTime < MISTAKE_COOLDOWN)
            {
                return;
            }
            
            MenuManager menuManager = MenuManager.Instance;
            menuManager.GetMenuOfType<PopupMenu>().CreatePopup(message, Popup.Level.Warning);
            MenuManager.Instance.GetMenuOfType<OverlayMenu>().PlayMistakeAnimation();
            
            JToken token = _gameMistakes[GetType().Name];
            if (ReferenceEquals(null, token))
            {
                throw new NullReferenceException();
            }
            
            token["is_completed"] = StateMachine.Instance.IsStateCompleted(StateName);
            ((JArray)token["feedback"])?.Add(message);
            
            SaveSystem.SaveFile(SaveSystem.GAME_MISTAKES, _gameMistakes.ToString());
            Debug.Log(SaveSystem.LoadFile(SaveSystem.GAME_MISTAKES));
            previousMistakeTime = DateTime.Now;
        }
    }
}
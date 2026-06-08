using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KattenKasteel.FSM;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom
{
    public static class FeedbackLogger
    {
        private static readonly string SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "saves");
        private static readonly string FEEDBACK_LOGS = Path.Combine(SAVE_FOLDER, "feedback-logs.json");
        private static JObject _jObject;

        static FeedbackLogger()
        {
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }

            if (File.Exists(FEEDBACK_LOGS))
            {
                _jObject = JObject.Parse(File.ReadAllText(FEEDBACK_LOGS));
                return;
            }

            GenerateEmptyFile();
        }

        public static void AddFeedback(string stateName, string message)
        {
            if (!_jObject.TryGetValue(stateName, out JToken token))
            {
                throw new InvalidOperationException("Could not find key:" + stateName);
            }

            ((JArray)token).Add(message);
            File.WriteAllText(FEEDBACK_LOGS, _jObject.ToString());
        }

        public static string[] GetFeedback(string stateName) => !_jObject.TryGetValue(stateName, out JToken token)
            ? throw new InvalidOperationException("Could not find key:" + stateName)
            : ((JArray)token).ToObject<string[]>();

        private static void GenerateEmptyFile()
        {
            _jObject = new JObject();

            IEnumerable<State> states = Resources.LoadAll<State>("States").Where(state => !state.IsParent);
            foreach (State state in states)
            {
                _jObject.Add(state.StateName, new JArray());
            }

            File.WriteAllText(FEEDBACK_LOGS, _jObject.ToString());
        }

        public static void Reset()
        {
            GenerateEmptyFile();
        }
    }
}
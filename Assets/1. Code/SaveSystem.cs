using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CleanRoom.StateMachine;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom
{
    public static class SaveSystem
    {
        public const string GAME_MISTAKES = "game-mistakes.json";
        private static readonly string SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "saves");

        static SaveSystem()
        {
            if (Directory.Exists(SAVE_FOLDER))
            {
                return;
            }

            Directory.CreateDirectory(SAVE_FOLDER);
        }

        public static void SaveFile(string filename, string content) =>
            File.WriteAllText(Path.Combine(SAVE_FOLDER, filename), content);


        public static string LoadFile(string filename)
        {
            string filepath = Path.Combine(SAVE_FOLDER, filename);
            return File.Exists(filepath) ? File.ReadAllText(filepath) : null;
        }

        public static void DeleteAllSaves()
        {
            foreach (string filename in Directory.GetFiles(SAVE_FOLDER))
            {
                File.Delete(Path.Combine(SAVE_FOLDER, filename));
            }
        }

        public static JObject GetGameMistakes()
        {
            if (File.Exists(Path.Combine(SAVE_FOLDER, GAME_MISTAKES)))
            {
                return JObject.Parse(LoadFile(GAME_MISTAKES));
            }
            
            Type[] gameStates = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.BaseType == typeof(GameState)).ToArray();

            JObject jObject = new JObject();
            
            foreach (Type gameState in gameStates)
            {
                jObject.Add(gameState.Name, 0);
            }
            
            SaveFile(GAME_MISTAKES, jObject.ToString());
            return jObject;
        }
    }
}
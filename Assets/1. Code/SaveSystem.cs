using System.IO;
using UnityEngine;

namespace CleanRoom
{
    public static class SaveSystem
    {
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
    }
}
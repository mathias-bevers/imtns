using System.IO;
using UnityEngine;

namespace CleanRoom.Utils
{
    public static class SaveSystem
    {
        private static readonly string SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "saves");

        public static void Save(string filename, string contents, bool append = false)
        {
            string filepath = Path.Combine(SAVE_FOLDER, filename);
            if (append)
            {
                File.AppendAllText(filepath, contents);
            }
            else
            {
                File.WriteAllText(filepath, contents);
            }
        }

        public static bool Load(string filename, out string contents)
        {
            string filepath = Path.Combine(SAVE_FOLDER, filename);
            if (!File.Exists(filepath))
            {
                contents = string.Empty;
                return false;
            }

            contents = File.ReadAllText(filepath);
            return true;
        }

        public static void Reset()
        {
            DirectoryInfo root = new(SAVE_FOLDER);
            foreach (FileInfo file in root.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in root.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
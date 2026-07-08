using System.IO;
using UnityEngine;

namespace Core.Save
{
    public static class SavePathProvider
    {
        public static string SettingsFile => "settings.json";

        public static string MetaFile => "meta.json";

        public static string SlotFile(string slotId) => Path.Combine("slots", $"{slotId}.json");
        
        public static string GetAbsolutePath(string relativePath) => Path.Combine(_rootDirectory, relativePath);
        private static string _rootDirectory => Path.Combine(Application.persistentDataPath, "save");

        
        public static void EnsureRootExists()
        {
            Directory.CreateDirectory(_rootDirectory);
            Directory.CreateDirectory(Path.Combine(_rootDirectory, "slots"));
        }
    }
}
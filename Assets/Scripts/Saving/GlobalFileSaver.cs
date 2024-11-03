using System.IO;
using UnityEngine;

namespace Saving
{
    public static class GlobalFileSaver
    {
        public static string Path => Application.persistentDataPath;

        public static string ReadFromDrive(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"Path: '{path}'");

            using var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            using var stream = new StreamReader(fs);
            var str = stream.ReadToEnd();
            return str;
        }

        public static void SaveToDrive(string content, string path)
        {
            if (File.Exists(path)) File.Delete(path);
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            using var stream = new StreamWriter(fs);
            stream.WriteLine(content);
            stream.Flush();
        }
    }
}
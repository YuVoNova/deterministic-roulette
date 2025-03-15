using System.IO;
using Context.Interfaces;
using UnityEngine;

namespace Context
{
    public class FileService : IFileService, IDisposableObject
    {
        public void Dispose()
        {
        }

        public string Load(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "");

            return File.ReadAllText(filePath);
        }

        public void Save(string fileName, string data)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(filePath, data);
        }
    }
}
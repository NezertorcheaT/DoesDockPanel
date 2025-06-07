using System;
using CustomHelper;
using UnityEngine;

namespace MiddleSpawn
{
    public class FileObject : IOpenableObject
    {
        public FileObject(FilePath currentPath)
        {
            CurrentPath = currentPath;
        }

        public FilePath CurrentPath { get; }

        public void Open(Action<IOpenablesContainer> updateContainer)
        {
            Debug.Log($"opened file {CurrentPath}");
            Helper.OpenWithDefaultProgram(CurrentPath);
        }
    }
}
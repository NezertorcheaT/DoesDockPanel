using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MiddleSpawn
{
    public class DirectoryObject : IOpenableObject, IOpenablesContainer
    {
        public DirectoryObject(FilePath currentPath, IOpenablesContainer parent)
        {
            CurrentPath = currentPath;
            Parent = parent;
        }

        public FilePath CurrentPath { get; }
        public IOpenablesContainer Parent { get; }

        public void Open(Action<IOpenablesContainer> updateContainer)
        {
            Debug.Log("opened folder");
            updateContainer(this);
        }

        public IEnumerator<IOpenableObject> GetEnumerator()
        {
            return Directory
                .EnumerateFileSystemEntries(CurrentPath, "**", new EnumerationOptions { IgnoreInaccessible = true })
                .Select(i => i
                    .Replace('/', Path.AltDirectorySeparatorChar)
                    .Replace('\\', Path.AltDirectorySeparatorChar)
                )
                .OrderBy(i => !Directory.Exists(i))
                .ThenBy(i => i
                    .Split(Path.AltDirectorySeparatorChar)
                    .Last()
                )
                .Select(i => Directory.Exists(i)
                    ? new DirectoryObject(i, this) as IOpenableObject
                    : new FileObject(i))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public void OpenAt(OpeningIndex index)
        {
            throw new NotImplementedException();
        }
    }
}
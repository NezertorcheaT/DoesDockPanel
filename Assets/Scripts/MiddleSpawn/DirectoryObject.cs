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
            Debug.Log($"opened folder {CurrentPath}");
            updateContainer(this);
        }

        public IEnumerable<(OpeningIndex, IOpenableObject)> EvaluateInners(IOpenablesContainer container) =>
            Directory
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
                .Select((i, ind) => Directory.Exists(i)
                    ? (new OpeningIndex(ind), new DirectoryObject(i, container) as IOpenableObject)
                    : (new OpeningIndex(ind), new FileObject(i)));

        public IEnumerator<(OpeningIndex, IOpenableObject)> GetEnumerator() => EvaluateInners(this).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void UseAt(OpeningIndex index, Action<IOpenablesContainer> updateContainer)
        {
            if (index.Back)
            {
                updateContainer(Parent);
                return;
            }

            var c = this.ToArray();
            if (c.Length - 1 < index.Index) return;
            c[index.Index].Item2.Open(updateContainer);
        }
    }
}
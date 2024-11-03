﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Files
{
    public static class FileObjectUtility
    {
        public static async Task Populate(ICollection<FileObject> collection, string root)
        {
            collection.Dispose();

            foreach (
                var (file, texture)
                in await Task.WhenAll(Directory
                    .EnumerateFileSystemEntries(root, "**", new EnumerationOptions { IgnoreInaccessible = true })
                    .Select(i => i
                        .Replace('/', Path.DirectorySeparatorChar)
                        .Replace('\\', Path.DirectorySeparatorChar)
                    )
                    .OrderBy(i => !Directory.Exists(i))
                    .ThenBy(i => i
                        .Split(Path.DirectorySeparatorChar)
                        .Last()
                    )
                    .Select(async i => (i, await FileThumbnail.GetThumbnail(i)))
                )
            )
            {
                if (!Directory.Exists(file))
                    collection.Add(new Link(texture, file));
                else
                {
                    var folder = new Folder(texture, file);
                    await folder.UpdateLinks();
                    collection.Add(folder);
                }
            }
        }

        public static void Dispose(this ICollection<FileObject> collection)
        {
            while (true)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    var fileObject = collection.First();
                    fileObject.Dispose();
                    collection.Remove(fileObject);
                }

                collection.Clear();
                if (collection.Count == 0) break;
            }
        }
    }
}
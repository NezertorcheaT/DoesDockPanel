using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Files
{
    public static class FileObjectUtility
    {
        public static bool IsExcluded(string file) =>
            Path.GetFileName(file).StartsWith($"{FileObject.ExcludedStarting}.");

        public static string NameAsExcluded(string file) => new StringBuilder()
            .Append(file.Replace(Path.GetFileName(file), ""))
            .Append(FileObject.ExcludedStarting)
            .Append(".")
            .Append(Path.GetFileName(file))
            .ToString();

        public static async Task Populate(ICollection<FileObject> collection, string root)
        {
            collection.Dispose();

            foreach (
                var (file, texture)
                in await Task.WhenAll(Directory
                    .EnumerateFileSystemEntries(root, "**", new EnumerationOptions { IgnoreInaccessible = true })
                    .Where(i => !AdvancedLink.IsPathToConfig(i))
                    .Where(i => !IsExcluded(i))
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
                if (Directory.Exists(file))
                {
                    var folder = new Folder(texture, file);
                    await folder.UpdateLinks();
                    collection.Add(folder);
                }
                else if (AdvancedLink.IsLinkAdvanced(file))
                    collection.Add(new AdvancedLink(texture, file));
                else
                    collection.Add(new Link(texture, file));
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
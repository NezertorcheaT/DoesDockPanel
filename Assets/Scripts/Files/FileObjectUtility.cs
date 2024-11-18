using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Files;

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

            var files = Directory
                .EnumerateFileSystemEntries(root, "**", new EnumerationOptions { IgnoreInaccessible = true })
                .Where(i => !LinkUI.IsPathToConfig(i))
                .Where(i => !IsExcluded(i))
                .Select(i => i
                    .Replace('/', Path.AltDirectorySeparatorChar)
                    .Replace('\\', Path.AltDirectorySeparatorChar)
                )
                .OrderBy(i => !Directory.Exists(i))
                .ThenBy(i => i
                    .Split(Path.AltDirectorySeparatorChar)
                    .Last()
                );

            foreach (var (file, texture) in await Task.WhenAll(files.Select(async i => (i, await FileThumbnail.GetThumbnail(i)))))
            {
                if (Directory.Exists(file))
                {
                    var folder = new Folder(texture, file);
                    await folder.UpdateLinks();
                    collection.Add(folder);
                    continue;
                }

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
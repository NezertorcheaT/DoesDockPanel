using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Saving;
using UnityEngine;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour, IEntriable
{
    private void Start()
    {
    }

    public abstract class FileObject
    {
        public Texture2D Image;
        public string File;
        public override string ToString() => $"{base.ToString()} {{ File: {File}, Image: {Image} }}";
    }

    public class Link : FileObject
    {
    }

    public class Folder : FileObject
    {
        public readonly ObservableList<FileObject> Links;

        public Folder(Texture2D image, string file)
        {
            Image = image;
            File = file;
            Links = new ObservableList<FileObject>();
        }

        public async Task UpdateLinks()
        {
            Links.Clear();
            await Populate(Links, File);
        }
    }

    public readonly ObservableList<FileObject> Links = new();

    private async void UpdateImages()
    {
        Links.Clear();
        await Populate(Links, ConfigEntry.Instance.LinksPath);
    }

    private static async Task Populate(ICollection<FileObject> collection, string root)
    {
        foreach (var file in Directory
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
                )
        {
            var texture = await FileThumbnail.GetThumbnail(file);
            if (!Directory.Exists(file))
                collection.Add(new Link { Image = texture, File = file });
            else
            {
                var folder = new Folder(texture, file);
                await folder.UpdateLinks();
                collection.Add(folder);
            }
        }
    }

    void IEntriable.Begin()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }
}
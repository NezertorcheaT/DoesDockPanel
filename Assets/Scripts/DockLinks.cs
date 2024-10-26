using System;
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

    public abstract class FileObject : IDisposable
    {
        public Texture2D Image;
        public string File;
        public override string ToString() => $"{base.ToString()} {{ File: {File}, Image: {Image} }}";
        public abstract void Dispose();
    }

    public class Link : FileObject
    {
        public override void Dispose()
        {
        }
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
            await Populate(Links, File);
        }

        public override void Dispose()
        {
            Links.Clear();
        }
    }

    public readonly ObservableList<FileObject> Links = new();

    public async void UpdateImages()
    {
        await Populate(Links, ConfigEntry.Instance.LinksPath);
    }

    private static async Task Populate(ICollection<FileObject> collection, string root)
    {
        for (var i = 0; i < collection.Count; i++)
        {
            var fileObject = collection.First();
            fileObject.Dispose();
            collection.Remove(fileObject);
        }

        Debug.Log(collection.Count);
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
            Debug.Log(file);
            if (!Directory.Exists(file))
                collection.Add(new Link { Image = texture, File = file });
            else
            {
                var folder = new Folder(texture, file);
                await folder.UpdateLinks();
                collection.Add(folder);
            }
        }

        Debug.Log(collection.Count);
    }

    void IEntriable.Begin()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }
}
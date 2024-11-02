using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Saving;
using UnityEngine;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour
{
    public abstract class FileObject : IDisposable, IEquatable<FileObject>
    {
        public bool Equals(FileObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return File == other.File;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is FileObject other && Equals(other);
        }

        public override int GetHashCode()
        {
            return File.GetHashCode();
        }

        public static bool operator ==(FileObject left, FileObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileObject left, FileObject right)
        {
            return !Equals(left, right);
        }

        public Texture2D Image;
        public string File;
        public override string ToString() => $"{base.ToString()} {{ File: {File}, Image: {Image} }}";
        public abstract void Dispose();
    }

    public class Link : FileObject
    {
        public Link(Texture2D image, string file)
        {
            Image = image;
            File = file;
        }

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
            Links.Dispose();
        }
    }

    public readonly ObservableList<FileObject> Links = new();

    private void Start()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }

    public async void UpdateImages()
    {
        await Populate(Links, ConfigEntry.Instance.LinksPath);
    }

    private void OnDestroy()
    {
        Links.Dispose();
    }

    private static async Task Populate(ICollection<FileObject> collection, string root)
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
}

public static class FileObjectUtility
{
    public static void Dispose(this ICollection<DockLinks.FileObject> collection)
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
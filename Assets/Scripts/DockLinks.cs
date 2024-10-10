using System.IO;
using Saving;
using UnityEngine;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour, IEntriable
{
    private void Start()
    {
    }

    public struct Link
    {
        public Texture2D Image;
        public string Path;
    }

    public ObservableList<Link> Links;

    private void UpdateImages()
    {
        Links = new ObservableList<Link>();
        foreach (var file in Directory.EnumerateFiles(ConfigEntry.Instance.LinksPath, "*.*",
                     new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true }))
        {
            var current = file.Replace('/', Path.DirectorySeparatorChar);
            current = current.Replace('\\', Path.DirectorySeparatorChar);
            var texture = FileThumbnail.GetThumbnail(current);
            Links.Add(new Link { Image = texture, Path = current });
        }
    }

    void IEntriable.Begin()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }
}
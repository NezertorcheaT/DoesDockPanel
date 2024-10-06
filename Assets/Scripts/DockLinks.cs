using System.IO;
using Saving;
using UnityEngine;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour, IEntriable
{
    private void Start()
    {
    }

    private void CheckThumbnail(string file)
    {
        var texture = FileThumbnail.GetThumbnail(file);
        Links.Add(new Link { Image = texture, Path = file });
    }

    public struct Link
    {
        public Texture2D Image;
        public string Path;
    }

    public ObservableList<Link> Links;

    private void RebuildImages()
    {
        foreach (var file in Directory.EnumerateFiles(ConfigEntry.Instance.LinksPath, "*.*",
                     new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true }))
        {
            CheckThumbnail(file);
        }
    }

    void IEntriable.Begin()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
    }
}
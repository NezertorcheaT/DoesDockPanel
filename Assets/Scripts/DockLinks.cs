using System.IO;
using Files;
using Saving;
using UnityEngine;
using UnityEngine.Rendering;

public class DockLinks : MonoBehaviour
{
    public readonly ObservableList<FileObject> Links = new();

    private void Start()
    {
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);
        UpdateImages();
    }

    public async void UpdateImages()
    {
        await FileObjectUtility.Populate(Links, ConfigEntry.Instance.LinksPath);
    }

    private void OnDestroy()
    {
        Links.Dispose();
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Files;
using Saving.Settings;
using UI.Files;
using UnityEngine;

public static class DockTextures
{
    public static IReadOnlyDictionary<FilePath, Texture2D> Textures { get; private set; }

    public static async Task Update() => Textures = await Generate();

    private static async Task<IReadOnlyDictionary<FilePath, Texture2D>> Generate()
    {
        Dictionary<FilePath, Texture2D> textures = new();
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);

        var files = Directory
            .EnumerateFileSystemEntries(ConfigEntry.Instance.LinksPath, "**",
                new EnumerationOptions { RecurseSubdirectories = true })
            .Where(i => !LinkUI.IsPathToConfig(i) && !FileObjectUtility.IsExcluded(i))
            .Select(i => i
                .Replace('/', Path.AltDirectorySeparatorChar)
                .Replace('\\', Path.AltDirectorySeparatorChar)
            );

        foreach (var (file, texture) in await Task.WhenAll(files.Select(async i =>
                     (i, await FileThumbnail.GetThumbnail(i)))))
            textures.Add(file, texture);
        return textures;
    }
}
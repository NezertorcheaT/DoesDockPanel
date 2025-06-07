using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Saving.Settings;
using UnityEngine;

public static class DockTextures
{
    private static IReadOnlyDictionary<FilePath, Texture2D> _textures;
    private static bool _texturesUpdating;

    public static Texture2D Get(FilePath path) => _texturesUpdating ? FileThumbnail.NullTexture : _textures[path];

    public static async UniTask Update()
    {
        _texturesUpdating = true;
        Dictionary<FilePath, Texture2D> textures = new();
        if (!Directory.Exists(ConfigEntry.Instance.LinksPath))
            Directory.CreateDirectory(ConfigEntry.Instance.LinksPath);

        var files = Directory
            .EnumerateFileSystemEntries(ConfigEntry.Instance.LinksPath, "**",
                new EnumerationOptions { RecurseSubdirectories = true })
            .Select(i => i
                .Replace('/', Path.AltDirectorySeparatorChar)
                .Replace('\\', Path.AltDirectorySeparatorChar)
            );

        foreach (var (file, texture) in await Task.WhenAll(Enumerable.Select(files, async i =>
                     (i, await FileThumbnail.GetThumbnail(i)))))
            textures.Add(file, texture);
        _textures = textures;
        _texturesUpdating = false;
    }
}
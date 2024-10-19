﻿using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using CustomHelper;
using Cysharp.Threading.Tasks;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class FileThumbnail
{
    public static readonly string CacheThumbnailFile =
        Path.Combine(Application.persistentDataPath, "cache.png")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);

    public static readonly string ThumbnailSolution =
        Path.Combine(Application.dataPath.Replace("Assets", "Thumbnails"), "Thumbnails.csproj")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
#if UNITY_EDITOR
    public static readonly string BuildArtifacts =
        Path.Combine(Application.dataPath.Replace("Assets", "Build"), $"{Application.productName}_Data/Thumbnails")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
#else
    public static readonly string BuildArtifacts =
        Path.Combine(Application.dataPath.Replace("Assets", "Build"), "Thumbnails")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
#endif

    public static readonly string ThumbnailsExecutable =
        Path.Combine(BuildArtifacts, "bin/Thumbnails/debug/Thumbnails.exe")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);

#if UNITY_EDITOR
    [MenuItem("File/Build Thumbnails", false, 3)]
    public static async Task RebuildThumbnailsSolution()
    {
        if (!Directory.Exists(BuildArtifacts))
            Directory.CreateDirectory(BuildArtifacts);
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"build \"{ThumbnailSolution}\" --artifacts-path \"{BuildArtifacts}\"";
            process.Start();
            await UniTask.WaitWhile(() => !process.HasExited);
        }

        if (Directory.Exists(Path.Combine(BuildArtifacts, "obj")))
            Directory.Delete(Path.Combine(BuildArtifacts, "obj"), true);
    }
#endif

    public static async Task<Texture2D> GetThumbnail(string filePath)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        using (var process = new Process())
        {
            process.StartInfo.FileName = ThumbnailsExecutable;
            process.StartInfo.Arguments = $"\"{filePath}\" \"{CacheThumbnailFile}\"";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            await UniTask.WaitWhile(() => !process.HasExited);
        }

        var bm = new Bitmap(CacheThumbnailFile);
        var thumbnail = bm.AsTexture2D();
        bm.Dispose();
        return thumbnail;
#else
        Debug.LogError("This functionality is only supported on Windows.");
        return null;
#endif
    }
}


#if UNITY_EDITOR
public class ThumbnailsSolutionBuilder : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public async void OnPostprocessBuild(BuildReport report)
    {
        await FileThumbnail.RebuildThumbnailsSolution();
    }
}
#endif

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Texture2D AsTexture2D(this Bitmap bitmap)
        {
            var texture = new Texture2D(bitmap.Width, bitmap.Height, TextureFormat.ARGB32, false);

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                texture.LoadImage(ms.ToArray());
            }

            if (bitmap.GetPixel(96, 96).A == 0 && bitmap.GetPixel(160, 160).A == 0)
            {
                texture.Apply();
                var newTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                newTexture.SetPixels(0, 0, 64, 64, texture.GetPixels(96, 96, 64, 64));
                newTexture.Apply();
                texture = newTexture;
            }

            if (texture.height == 64)
                texture.filterMode = FilterMode.Point;

            texture.Apply();
            return texture;
        }
    }
}
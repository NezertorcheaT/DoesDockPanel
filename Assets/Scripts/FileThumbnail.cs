using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Graphics = System.Drawing.Graphics;

public static class FileThumbnail
{
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr ExtractIcon(IntPtr hInstance, string lpszFile, int nIconIndex);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    public static Texture2D GetThumbnail(string filePath, int iconSize = 512)
    {
        var texture = new Texture2D(iconSize, iconSize, TextureFormat.ARGB32, false);

        using (var ms = new MemoryStream())
        {
            var bmp = Icon.ExtractAssociatedIcon(filePath).ToBitmap();
            bmp.Save(ms, ImageFormat.Png);
            texture.LoadImage(ms.ToArray());
        }

        texture.Apply();
        return texture;
    }
}
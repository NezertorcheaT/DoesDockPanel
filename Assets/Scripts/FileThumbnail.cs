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

    public static Texture2D GetThumbnail(string filePath, int iconSize = 256)
    {
        var iconHandle = ExtractIcon(IntPtr.Zero, filePath, 0);

        if (iconHandle == IntPtr.Zero)
            throw new Exception("I dono");
        var texture = new Texture2D(iconSize, iconSize, TextureFormat.ARGB32, false);

        using (var bmp = new Bitmap(iconSize, iconSize))
        {
            using (var g = Graphics.FromImage(bmp))
            {
                var icon = Icon.FromHandle(iconHandle);
                g.DrawIcon(icon, new Rectangle(0, 0, iconSize, iconSize));
            }

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                texture.LoadImage(ms.ToArray());
            }
        }

        DestroyIcon(iconHandle);
        return texture;
    }
}
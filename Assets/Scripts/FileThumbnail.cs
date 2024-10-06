using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

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

        // Create a temporary RenderTexture to draw the icon onto
        var renderTexture = new RenderTexture(iconSize, iconSize, 0);
        Graphics.SetRenderTarget(renderTexture);
        GL.Clear(true, true, Color.clear);

        // Draw the icon using a GUI.DrawTexture call
        GUI.DrawTexture(new Rect(0, 0, iconSize, iconSize), GetIconTexture(iconHandle));
        Graphics.SetRenderTarget(null);

        // Read the contents of the RenderTexture into a Texture2D
        var texture = new Texture2D(iconSize, iconSize, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        texture.Apply();

        // Release resources
        RenderTexture.ReleaseTemporary(renderTexture);
        DestroyIcon(iconHandle);

        return texture;
    }

    // Helper function to convert an icon handle to a Texture2D
    private static Texture2D GetIconTexture(IntPtr iconHandle)
    {
        // Create a temporary MemoryStream to store the icon data
        using (var ms = new MemoryStream())
        {
            // Get the icon bitmap from the handle
            var icon = System.Drawing.Icon.FromHandle(iconHandle);

            // Save the icon to the MemoryStream as a PNG
            icon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // Create a Texture2D from the MemoryStream data
            var texture = new Texture2D(1, 1);
            texture.LoadImage(ms.ToArray());
            return texture;
        }
    }
}
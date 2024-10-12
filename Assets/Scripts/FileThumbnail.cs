using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using CustomHelper;
using UnityEngine;
using Graphics = System.Drawing.Graphics;

public static class FileThumbnail
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEINFO
    {
        public SHFILEINFO(bool b)
        {
            hIcon = IntPtr.Zero;
            iIcon = 0;
            dwAttributes = 0;
            szDisplayName = "";
            szTypeName = "";
        }

        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
        public string szDisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_TYPE)]
        public string szTypeName;
    }

    private const int MAX_PATH = 260;
    private const int MAX_TYPE = 80;

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHGetFileInfo(
        string pszPath,
        int dwFileAttributes,
        out SHFILEINFO psfi,
        uint cbfileInfo,
        SHGFI uFlags);

    [Flags]
    private enum SHGFI : int
    {
        Icon = 0x000000100,
        LargeIcon = 0x000000000,
        SmallIcon = 0x000000001,
        OpenIcon = 0x000000002,
        ShellIconSize = 0x000000004,
        UseFileAttributes = 0x000000010,
        SysIconIndex = 0x000004000,
    }

    public static Texture2D GetThumbnail(string filePath)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

        var info = new SHFILEINFO(true);
        var cbFileInfo = Marshal.SizeOf(info);

        // Try getting the largest icon first (SHGFI.LargeIcon)
        const SHGFI flagsLarge = SHGFI.Icon | SHGFI.LargeIcon | SHGFI.SysIconIndex | SHGFI.UseFileAttributes;
        SHGetFileInfo(filePath, 256, out info, (uint)cbFileInfo, flagsLarge);
        var hIcon = info.hIcon;
        if (hIcon == IntPtr.Zero)
        {
            Debug.LogError("Failed to extract icon from file: " + filePath);
            return null;
        }

        var icon = Icon.FromHandle(hIcon);
        var bmp = new Bitmap(icon.Width, icon.Height);
        using (var g = Graphics.FromImage(bmp))
        {
            g.DrawIcon(icon, 0, 0);
        }

        DestroyIcon(hIcon);
        return bmp.AsTexture2D();
#else
        Debug.LogError("This functionality is only supported on Windows.");
        return null;
#endif
    }
}

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

            texture.Apply();
            return texture;
        }
    }
}
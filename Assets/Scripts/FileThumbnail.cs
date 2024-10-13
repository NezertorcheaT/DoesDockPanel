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
    private static extern int SHDefExtractIconA(
        string pszIconFile,
        int iIndex,
        SHGFI uFlags,
        IntPtr phiconLarge,
        IntPtr phiconSmall,
        uint nIconSize
    );

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHGetFileInfo(
        string pszPath,
        int dwFileAttributes,
        out SHFILEINFO psfi,
        uint cbfileInfo,
        SHGFI uFlags);

    [Flags]
    enum SHGFI : int
    {
        /// <summary>get icon</summary>
        Icon = 0x000000100,

        /// <summary>get display name</summary>
        DisplayName = 0x000000200,

        /// <summary>get type name</summary>
        TypeName = 0x000000400,

        /// <summary>get attributes</summary>
        Attributes = 0x000000800,

        /// <summary>get icon location</summary>
        IconLocation = 0x000001000,

        /// <summary>return exe type</summary>
        ExeType = 0x000002000,

        /// <summary>get system icon index</summary>
        SysIconIndex = 0x000004000,

        /// <summary>put a link overlay on icon</summary>
        LinkOverlay = 0x000008000,

        /// <summary>show icon in selected state</summary>
        Selected = 0x000010000,

        /// <summary>get only specified attributes</summary>
        Attr_Specified = 0x000020000,

        /// <summary>get large icon</summary>
        LargeIcon = 0x000000000,

        /// <summary>get small icon</summary>
        SmallIcon = 0x000000001,

        /// <summary>get open icon</summary>
        OpenIcon = 0x000000002,

        /// <summary>get shell size icon</summary>
        ShellIconSize = 0x000000004,

        /// <summary>pszPath is a pidl</summary>
        PIDL = 0x000000008,

        /// <summary>use passed dwFileAttribute</summary>
        UseFileAttributes = 0x000000010,

        /// <summary>apply the appropriate overlays</summary>
        AddOverlays = 0x000000020,

        /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
        OverlayIndex = 0x000000040,
    }


    public static Texture2D GetThumbnail(string filePath)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

        var info = new SHFILEINFO(true);
        var cbFileInfo = Marshal.SizeOf(info);
        var hIcon = new IntPtr(0);

        const SHGFI flagsLarge = SHGFI.Icon | SHGFI.LargeIcon | SHGFI.UseFileAttributes | SHGFI.ShellIconSize;
        var ind = SHGetFileInfo(filePath, 256, out info, (uint)cbFileInfo, SHGFI.SysIconIndex);
        SHGetFileInfo(filePath, 256, out info, (uint)cbFileInfo, flagsLarge);
        hIcon = info.hIcon;
        SHDefExtractIconA(filePath, ind, flagsLarge, hIcon, new IntPtr(0), 256);
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
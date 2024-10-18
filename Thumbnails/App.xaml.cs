using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;

namespace Thumbnails;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        if (e.Args.Length != 2)
        {
            Shutdown(1);
            return;
        }

        var file = e.Args[0];
        var to = e.Args[1];
        if (!File.Exists(to))
            File.Create(to);
        var image = GetIcon(file);
        using (var s = new FileStream(to, FileMode.Open))
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(s);
        }

        Shutdown();
    }

    private static BitmapSource GetIcon(string file) => ShellFile.FromFilePath(file).Thumbnail.LargeBitmapSource;

    public static void OpenWithDefaultProgram(string file)
    {
        using var filerOpener = new Process();

        filerOpener.StartInfo.FileName = "explorer";
        filerOpener.StartInfo.Arguments = file;
        filerOpener.Start();
    }
}
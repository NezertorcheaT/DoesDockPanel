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
            File.Create(to).Dispose();

        var image = File.Exists(file)
            ? ShellFile.FromFilePath(file).Thumbnail.LargeBitmapSource
            : ShellFileSystemFolder.FromFolderPath(file).Thumbnail.LargeBitmapSource;

        using (var s = new FileStream(to, FileMode.Open))
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(s);
        }

        Shutdown();
    }
}
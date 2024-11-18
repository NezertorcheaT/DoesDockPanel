using System.IO;
using CustomHelper;
using Files;
using R3;
using Saving.Links;

namespace UI.Files
{
    public class LinkUI : FileUI
    {
        public const string ConfigExtension = "conf";
        public FilePath ConfigFile => $"{CurrentFile.File}.{ConfigExtension}";
        public LinkConfig Config { get; private set; }

        public void Initialize(Link link)
        {
            Initialize(link as FileObject);

            var saver = new LinkConfigFileSaver();
            Config = new LinkConfig(saver, this);
            if (File.Exists(ConfigFile))
                Config = (LinkConfig)Config.Deconvert(saver.Read(ConfigFile), saver);

            LeftClick.Subscribe(_ => Helper.OpenWithDefaultProgram(Config.LeftClickAction));
            MiddleClick.Subscribe(_ => Helper.OpenWithDefaultProgram(Config.MiddleClickAction));
            RightClick.Subscribe(_ => Helper.OpenWithDefaultProgram(Config.RightClickAction));
        }

        public static bool LinkHasConfig(string file) => File.Exists($"{file}.{ConfigExtension}");
        public static bool IsPathToConfig(string file) => file.EndsWith(ConfigExtension);
    }
}
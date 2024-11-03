using Saving.Links;
using UnityEngine;

namespace Files
{
    public class AdvancedLink : FileObject
    {
        public const string ConfigExtension = "conf";
        public string ConfigPath => $"{File}.{ConfigExtension}";
        private LinkConfig Config { get; }

        public AdvancedLink(Texture2D image, string file) : base(image, file)
        {
            var saver = new LinkConfigFileSaver();
            if (!IsLinkAdvanced(file))
            {
                Debug.LogError($"File \"{file}\" is not advanced link, config was probably deleted in the process");
                return;
            }

            Config = new LinkConfig(saver, this);
        }

        public static bool IsLinkAdvanced(string file) => System.IO.File.Exists($"{file}.{ConfigExtension}");
        public static bool IsPathToConfig(string file) => file.EndsWith(ConfigExtension);

        public override void Dispose()
        {
        }
    }
}
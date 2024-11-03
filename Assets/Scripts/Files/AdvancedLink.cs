using UnityEngine;

namespace Files
{
    public class AdvancedLink : FileObject
    {
        public const string Extension = "conf";

        public AdvancedLink(Texture2D image, string file) : base(image, file)
        {
        }

        public static bool IsLinkAdvanced(string file) => System.IO.File.Exists($"{file}.{Extension}");

        public override void Dispose()
        {
        }
    }
}
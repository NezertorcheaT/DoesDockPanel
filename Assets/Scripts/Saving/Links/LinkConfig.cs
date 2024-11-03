using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Files;
using Saving.Settings;

namespace Saving.Links
{
    [Serializable]
    public class LinkConfig : IFileSaver<string>.ISavable
    {
        public string LinksPath
        {
            get => _linksPath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            set
            {
                _linksPath = value
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                _saver.Save(this);
            }
        }

        private string _linksPath = $"{GlobalFileSaver.Path}{Path.DirectorySeparatorChar}Links";

        private IFileSaver<string> _saver;
        public AdvancedLink AssociatedLink { get;  }

        [JsonConstructor]
        private LinkConfig(
            string linksPath
        )
        {
            _linksPath = linksPath;
        }

        public LinkConfig(LinkConfigFileSaver saver,AdvancedLink link)
        {
            _saver = saver;
            AssociatedLink = link;
        }

        string IFileSaver<string>.ISavable.Convert() =>
            JsonSerializer.Serialize(this, Config.SerializerOptions);

        public IFileSaver<string>.ISavable Deconvert(string converted, IFileSaver<string> saver)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<LinkConfig>(converted, Config.SerializerOptions);
                if (deserialized is null)
                    throw new ArgumentException(
                        $"Converted string '{converted}' is not LinkConfig and can't be deserialized");
                deserialized._saver = saver;
                return deserialized;
            }
            catch
            {
                var config = new LinkConfig(
                    $"{GlobalFileSaver.Path}{Path.DirectorySeparatorChar}Links"
                );
                config._saver = saver;
                config._saver.Save(config);
                return config;
            }
        }
    }
}
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Saving.Settings;
using UI.Files;

namespace Saving.Links
{
    [Serializable]
    public class LinkConfig : IFileSaver<string>.ISavable
    {
        public string LeftClickAction
        {
            get => _leftClickAction
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            set
            {
                _leftClickAction = value
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                _saver.Save(this);
            }
        }

        private string _leftClickAction = "";

        public string MiddleClickAction
        {
            get => _middleClickAction
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            set
            {
                _middleClickAction = value
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                _saver.Save(this);
            }
        }

        private string _middleClickAction = "";

        public string RightClickAction
        {
            get => _rightClickAction
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            set
            {
                _rightClickAction = value
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                _saver.Save(this);
            }
        }

        private string _rightClickAction = "";

        private IFileSaver<string> _saver;
        [JsonIgnore] public LinkUI AssociatedLink { get; private set; }

        [JsonIgnore]
        public bool ActionEmpty =>
            string.IsNullOrWhiteSpace(_leftClickAction) &&
            string.IsNullOrWhiteSpace(_rightClickAction) &&
            string.IsNullOrWhiteSpace(_middleClickAction);

        [JsonConstructor]
        private LinkConfig(
            string leftClickAction = "",
            string middleClickAction = "",
            string rightClickAction = ""
        )
        {
            _leftClickAction = leftClickAction;
            _middleClickAction = middleClickAction;
            _rightClickAction = rightClickAction;
        }

        public LinkConfig(IFileSaver<string> saver, LinkUI link)
        {
            _saver = saver;
            AssociatedLink = link;
            if (ActionEmpty)
                _leftClickAction = AssociatedLink.CurrentFile.File;
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
                deserialized.AssociatedLink = AssociatedLink;
                if (deserialized.ActionEmpty)
                    deserialized._leftClickAction = AssociatedLink.CurrentFile.File;
                return deserialized;
            }
            catch
            {
                var config = new LinkConfig(saver, AssociatedLink);
                config._saver.Save(config);
                return config;
            }
        }
    }
}
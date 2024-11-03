using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Files;
using Saving.Settings;

namespace Saving.Links
{
    [Serializable]
    public class LinkConfig : IFileSaver<string>.ISavable
    {
        public string ClickAction
        {
            get => _clickAction;
            set
            {
                _clickAction = value;
                _saver.Save(this);
            }
        }

        private string _clickAction = "";

        public string DoubleClickAction
        {
            get => _doubleClickAction;
            set
            {
                _doubleClickAction = value;
                _saver.Save(this);
            }
        }

        private string _doubleClickAction = "";

        public string RightClickAction
        {
            get => _rightClickAction;
            set
            {
                _rightClickAction = value;
                _saver.Save(this);
            }
        }

        private string _rightClickAction = "";

        private IFileSaver<string> _saver;
        [JsonIgnore] public AdvancedLink AssociatedLink { get; private set; }

        [JsonConstructor]
        private LinkConfig(
            string clickAction = "",
            string doubleClickAction = "",
            string rightClickAction = ""
        )
        {
            _clickAction = clickAction;
            _doubleClickAction = doubleClickAction;
            _rightClickAction = rightClickAction;
        }

        public LinkConfig(IFileSaver<string> saver, AdvancedLink link)
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
                deserialized.AssociatedLink = AssociatedLink;
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
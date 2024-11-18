using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Files;
using Saving.Settings;
using UI.Files;
using UnityEngine;

namespace Saving.Links
{
    [Serializable]
    public class LinkConfig : IFileSaver<string>.ISavable
    {
        public FilePath LeftClickAction
        {
            get => _leftClickAction;
            set
            {
                _leftClickAction = value;
                _saver.Save(this);
            }
        }

        private FilePath _leftClickAction = FilePath.Empty;

        public FilePath MiddleClickAction
        {
            get => _middleClickAction;
            set
            {
                _middleClickAction = value;
                _saver.Save(this);
            }
        }

        private FilePath _middleClickAction = FilePath.Empty;

        public FilePath RightClickAction
        {
            get => _rightClickAction;
            set
            {
                _rightClickAction = value;
                _saver.Save(this);
            }
        }

        private FilePath _rightClickAction = FilePath.Empty;

        private IFileSaver<string> _saver;
        [JsonIgnore] public LinkUI AssociatedLink { get; private set; }

        [JsonIgnore]
        public bool ActionEmpty =>
            LeftClickAction.IsEmpty &&
            RightClickAction.IsEmpty &&
            MiddleClickAction.IsEmpty;

        [JsonConstructor]
        private LinkConfig(
            FilePath leftClickAction,
            FilePath middleClickAction,
            FilePath rightClickAction
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
            catch (Exception e)
            {
                Debug.LogException(e);
                var config = new LinkConfig(saver, AssociatedLink);
                config._saver.Save(config);
                return config;
            }
        }
    }
}
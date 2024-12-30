using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Saving.Settings;
using UI.Files;
using UnityEngine;

namespace Saving.Folders
{
    [Serializable]
    public class FolderConfig : IFileSaver<string>.ISavable
    {
        public float ContainerHeight
        {
            get => _containerHeight;
            set
            {
                _containerHeight = value;
                _saver.Save(this);
            }
        }

        private float _containerHeight = 400;

        public float ContainerWidth
        {
            get => _containerWidth;
            set
            {
                _containerWidth = value;
                _saver.Save(this);
            }
        }

        private float _containerWidth = 500;

        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                _saver.Save(this);
            }
        }

        private Vector2 _offset = Vector2.zero;

        public bool IsUsingCustomIcon
        {
            get => _isUsingCustomIcon;
            set
            {
                _isUsingCustomIcon = value;
                _saver.Save(this);
            }
        }

        private bool _isUsingCustomIcon;

        private IFileSaver<string> _saver;
        [JsonIgnore] public IConfigurableFileUI<FolderConfig> AssociatedFile { get; private set; }

        [JsonConstructor]
        private FolderConfig(
            Vector2 offset,
            float containerWidth = 500,
            float containerHeight = 400,
            bool isUsingCustomIcon = false
        )
        {
            _containerWidth = containerWidth;
            _containerHeight = containerHeight;
            _offset = offset;
            _isUsingCustomIcon = isUsingCustomIcon;
        }

        public FolderConfig(IFileSaver<string> saver, IConfigurableFileUI<FolderConfig> file)
        {
            _saver = saver;
            AssociatedFile = file;
        }

        string IFileSaver<string>.ISavable.Convert() =>
            JsonSerializer.Serialize(this, Config.SerializerOptions);

        public IFileSaver<string>.ISavable Deconvert(string converted, IFileSaver<string> saver)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<FolderConfig>(converted, Config.SerializerOptions);
                if (deserialized is null)
                    throw new ArgumentException(
                        $"Converted string '{converted}' is not FolderConfig and can't be deserialized");
                deserialized._saver = saver;
                deserialized.AssociatedFile = AssociatedFile;
                return deserialized;
            }
            catch
            {
                var config = new FolderConfig(saver, AssociatedFile);
                config._saver.Save(config);
                return config;
            }
        }
    }
}
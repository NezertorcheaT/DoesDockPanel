using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Saving.Converters;
using UnityEngine;

namespace Saving
{
    /// <summary>
    /// ну это типа настройки, они сделаны так, что если вы измените любое поле, оно автоматически сохранится на диске
    /// </summary>
    [Serializable]
    public class Config : IFileSaver<string>.ISavable
    {
        public string LinksPath
        {
            get => _linksPath;
            set
            {
                _linksPath = value;
                _saver.Save(this);
            }
        }

        private string _linksPath = $"{GlobalFileSaver.Path}/Links";

        public TextAnchor TextAnchor
        {
            get => _textAnchor;
            set
            {
                _textAnchor = value;
                _saver.Save(this);
            }
        }

        private TextAnchor _textAnchor = TextAnchor.UpperCenter;

        public FolderSide FolderItemsPosition
        {
            get => _folderItemsPosition;
            set
            {
                _folderItemsPosition = value;
                _saver.Save(this);
            }
        }

        private FolderSide _folderItemsPosition = FolderSide.Down;


        public bool IsVertical
        {
            get => _isVertical;
            set
            {
                _isVertical = value;
                _saver.Save(this);
            }
        }

        private bool _isVertical;

        public static JsonSerializerOptions SerializerOptions => new()
        {
            Converters =
            {
                new Vector2Converter(),
                new Vector3Converter(),
            },
            WriteIndented = true,
        };

        private IFileSaver<string> _saver;

        [JsonConstructor]
        private Config(string linksPath, TextAnchor textAnchor, bool isVertical, FolderSide folderItemsPosition)
        {
            _linksPath = linksPath;
            _textAnchor = textAnchor;
            _isVertical = isVertical;
            _folderItemsPosition = folderItemsPosition;
        }

        public Config(ConfigFileSaver saver)
        {
            _saver = saver;
        }

        string IFileSaver<string>.ISavable.Convert() =>
            JsonSerializer.Serialize(this, SerializerOptions);

        public IFileSaver<string>.ISavable Deconvert(string converted, IFileSaver<string> saver)
        {
            var deserialized = JsonSerializer.Deserialize<Config>(converted, SerializerOptions);
            if (deserialized is null)
                throw new ArgumentException($"Converted string '{converted}' is not Config and can't be deserialized");
            deserialized._saver = saver;
            return deserialized;
        }
    }

    public enum FolderSide
    {
        Right,
        Left,
        Down,
        Up
    }
}
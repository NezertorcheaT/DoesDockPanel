using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Input;
using MiddleSpawn;
using Saving.Converters;
using UnityEngine;

namespace Saving.Settings
{
    /// <summary>
    /// ну это типа настройки, они сделаны так, что если вы измените любое поле, оно автоматически сохранится на диске
    /// </summary>
    [Serializable]
    public class Config : IFileSaver<string>.ISavable
    {
        public FilePath LinksPath
        {
            get => _linksPath;
            set
            {
                _linksPath = value;
                _saver.Save(this);
            }
        }

        private FilePath _linksPath = $"{GlobalFileSaver.Path}{Path.AltDirectorySeparatorChar}Links";

        public Keymap OpenKeymap
        {
            get => _openKeymap;
            set
            {
                _openKeymap = value;
                _saver.Save(this);
            }
        }

        private Keymap _openKeymap = new(
            $"{WindowsInput.Keys.Control.ToString()}," +
            $"{WindowsInput.Keys.Alt.ToString()}," +
            $"{WindowsInput.Keys.Space.ToString()}"
        );

        public static JsonSerializerOptions SerializerOptions => new()
        {
            Converters =
            {
                new Vector2Converter(),
                new Vector3Converter(),
                new FilePathConverter(),
                new KeymapConverter(),
            },
            WriteIndented = true,
        };

        private IFileSaver<string> _saver;

        [JsonConstructor]
        private Config(
            FilePath linksPath,
            Keymap openKeymap
        )
        {
            _linksPath = linksPath;
            _openKeymap = openKeymap;
        }

        public Config(ConfigFileSaver saver)
        {
            _saver = saver;
        }

        string IFileSaver<string>.ISavable.Convert() =>
            JsonSerializer.Serialize(this, SerializerOptions);

        public IFileSaver<string>.ISavable Deconvert(string converted, IFileSaver<string> saver)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<Config>(converted, SerializerOptions);
                if (deserialized is null)
                    throw new ArgumentException(
                        $"Converted string '{converted}' is not Config and can't be deserialized");
                deserialized._saver = saver;
                return deserialized;
            }
            catch
            {
                var config = new Config(
                    $"{GlobalFileSaver.Path}{Path.AltDirectorySeparatorChar}Links",
                    new Keymap(
                        $"{WindowsInput.Keys.Control.ToString()}," +
                        $"{WindowsInput.Keys.Alt.ToString()}," +
                        $"{WindowsInput.Keys.Space.ToString()}"
                    )
                );
                config._saver = saver;
                config._saver.Save(config);
                return config;
            }
        }
    }
}
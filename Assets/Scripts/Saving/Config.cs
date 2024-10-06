using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Saving.Converters;

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
        private Config(string linksPath)
        {
            _linksPath = linksPath;
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
}
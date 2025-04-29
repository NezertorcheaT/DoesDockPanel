using System;
using System.Text.Json;

namespace Saving.Converters
{
    public class FilePathConverter : System.Text.Json.Serialization.JsonConverter<FilePath>
    {
        public override FilePath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
        {
            writer.WriteRawValue($"\"{value}\"", true);
        }
    }
}
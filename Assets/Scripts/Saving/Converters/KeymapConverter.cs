using System;
using System.Collections.Generic;
using System.Text.Json;
using Input;
using MiddleSpawn;

namespace Saving.Converters
{
    public class KeymapConverter : System.Text.Json.Serialization.JsonConverter<Keymap>
    {
        public override Keymap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            return reader.TokenType != JsonTokenType.String
                ? new Keymap()
                : new Keymap(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Keymap value, JsonSerializerOptions options)
        {
            writer.WriteRawValue($"\"{value}\"", true);
        }
    }
}
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
            List<WindowsInput.Keys> keyset = new();

            reader.Read();
            var tokenType = reader.TokenType;
            if (tokenType != JsonTokenType.String) return new Keymap(keyset);
            var a = reader.GetString();
            foreach (var c in a.Split(','))
            {
                if (Enum.TryParse(c, out WindowsInput.Keys key))
                    keyset.Add(key);
            }

            return new Keymap(keyset);
        }

        public override void Write(Utf8JsonWriter writer, Keymap value, JsonSerializerOptions options)
        {
            writer.WriteRawValue($"\"{value}\"", true);
        }
    }
}
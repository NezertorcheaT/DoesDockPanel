using System;
using System.Text.Json;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector2Converter : System.Text.Json.Serialization.JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var v = new Vector2();
            while (reader.Read())
            {
                var tokenType = reader.TokenType;

                if (tokenType == JsonTokenType.EndObject)
                    return v;
                if (tokenType != JsonTokenType.PropertyName)
                    continue;

                if (reader.ValueTextEquals("x"))
                {
                    reader.Read();
                    v.x = reader.GetSingle();
                }
                else if (reader.ValueTextEquals("y"))
                {
                    reader.Read();
                    v.y = reader.GetSingle();
                }
            }

            return v;
        }


        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonUtility.ToJson(value));
        }
    }
}
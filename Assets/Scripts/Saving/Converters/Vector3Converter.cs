using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var v = new Vector3();
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
                else if (reader.ValueTextEquals("z"))
                {
                    reader.Read();
                    v.z = reader.GetSingle();
                }
            }

            return v;
        }


        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonUtility.ToJson(value));
        }
    }
}
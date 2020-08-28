using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zwyssigly.Functional.SystemTextJson
{
    public class OptionConverter<T> : JsonConverter<Option<T>>
    {
        public override Option<T> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) {
                reader.Read();
                return Option.None<T>();
            }

            return Option.Some(((JsonConverter<T>)options.GetConverter(typeof(T))).Read(ref reader, typeof(T), options));
        }

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            value.Match(
                some => ((JsonConverter<T>)options.GetConverter(typeof(T))).Write(writer, some, options),
                () => writer.WriteNullValue()
            );
        }
    }
}

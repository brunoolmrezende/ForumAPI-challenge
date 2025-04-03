using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Forum.API.Converters
{
    public partial class StringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()?.Trim();

            if (value == null) return null;

            return RemoveExtraWhiteSpaces().Replace(value, " ");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex RemoveExtraWhiteSpaces();
    }
}

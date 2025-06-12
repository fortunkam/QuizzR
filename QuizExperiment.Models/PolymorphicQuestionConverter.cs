using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace QuizExperiment.Models
{
    public class PolymorphicQuestionConverter : JsonConverter<Question>
    {
        public override Question? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("questionType", out var typeProp))
                {
                    var type = typeProp.GetString();
                    if (type == "multipleChoice")
                        return JsonSerializer.Deserialize<MultipleChoiceQuestion>(root.GetRawText(), options);
                    if (type == "trueFalse")
                        return JsonSerializer.Deserialize<TrueFalseQuestion>(root.GetRawText(), options);
                }
                // Fallback: treat as MultipleChoiceQuestion (legacy)
                return JsonSerializer.Deserialize<MultipleChoiceQuestion>(root.GetRawText(), options);
            }
        }

        public override void Write(Utf8JsonWriter writer, Question value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            // Write the type discriminator
            if (value is MultipleChoiceQuestion)
                writer.WriteString("questionType", "multipleChoice");
            else if (value is TrueFalseQuestion)
                writer.WriteString("questionType", "trueFalse");
            // Write all other properties as named properties
            var type = value.GetType();
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0)
                    continue;
                var propValue = prop.GetValue(value);
                var propName = prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).Length > 0
                    ? ((JsonPropertyNameAttribute)prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true)[0]).Name
                    : prop.Name;
                writer.WritePropertyName(propName);
                JsonSerializer.Serialize(writer, propValue, propValue?.GetType() ?? typeof(object), options);
            }
            writer.WriteEndObject();
        }
    }
}

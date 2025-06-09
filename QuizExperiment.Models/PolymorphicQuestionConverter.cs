using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}

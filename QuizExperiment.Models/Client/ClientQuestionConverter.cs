using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace QuizExperiment.Models.Client
{
    public class ClientQuestionConverter : JsonConverter<ClientQuestion>
    {
        public override ClientQuestion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                return JsonSerializer.Deserialize<ClientQuestion>(root.GetRawText(), options);
            }
        }

        public override void Write(Utf8JsonWriter writer, ClientQuestion value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<ClientQuestion>(writer, value, options);
        }
    }

    public class ClientMultipleChoiceQuestionConverter : JsonConverter<ClientMultipleChoiceQuestion>
    {
        public override ClientMultipleChoiceQuestion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                return JsonSerializer.Deserialize<ClientMultipleChoiceQuestion>(root.GetRawText(), options);
            }
        }

        public override void Write(Utf8JsonWriter writer, ClientMultipleChoiceQuestion value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<ClientQuestion>(writer, value, options);
        }
    }
}

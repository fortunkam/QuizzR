using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class PolymorphicQuestionListConverter : JsonConverter<List<Question>>
    {
        public override List<Question>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var questions = new List<Question>();
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();
            var itemConverter = new PolymorphicQuestionConverter();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;
                questions.Add(itemConverter.Read(ref reader, typeof(Question), options)!);
            }
            return questions;
        }

        public override void Write(Utf8JsonWriter writer, List<Question> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var itemConverter = new PolymorphicQuestionConverter();
            foreach (var question in value)
            {
                itemConverter.Write(writer, question, options);
            }
            writer.WriteEndArray();
        }
    }
}

using System.Text.Json;
using Xunit;

namespace QuizExperiment.Models.Test
{
    public class JsonSerializationTests
    {
        [Fact]
        public void DeserializeAndReserialize_V1Schema_PrintsOutput()
        {
            // Arrange
            var baseDir = AppContext.BaseDirectory;
            var jsonPath = Path.Combine(baseDir, "TestData", "v1SchemaQuestionSet.json");
            if (!File.Exists(jsonPath))
            {
                // Try to walk up to the project directory if running from bin/Debug/net8.0
                jsonPath = Path.Combine(baseDir, "..", "..", "..", "TestData", "v1SchemaQuestionSet.json");
                jsonPath = Path.GetFullPath(jsonPath);
            }
            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PolymorphicQuestionConverter());
            options.Converters.Add(new PolymorphicQuestionListConverter());
            options.PropertyNamingPolicy = null;

            // Act
            var questionSet = JsonSerializer.Deserialize<QuestionSet>(json, options);
            var outputJson = JsonSerializer.Serialize(questionSet, options);

            // Output
            Console.WriteLine("Deserialized and re-serialized JSON:\n" + outputJson);

            // Validate that each question has a questionType property
            using var doc = JsonDocument.Parse(outputJson);
            var questions = doc.RootElement.GetProperty("questions");
            foreach (var question in questions.EnumerateArray())
            {
                Assert.True(question.TryGetProperty("questionType", out _),
                    $"A question is missing the 'questionType' property: {question}");
            }
        }

        [Fact]
        public void SerializingMultipleQuestionTypes()
        {
            // Arrange
            var baseDir = AppContext.BaseDirectory;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PolymorphicQuestionConverter());
            options.Converters.Add(new PolymorphicQuestionListConverter());
            options.PropertyNamingPolicy = null;

            // Act
            var questionSet = new QuestionSet
            {
                Title = "Sample Quiz",
                Questions = new List<Question>
                {
                    new MultipleChoiceQuestion
                    {
                        Title = "What is the capital of France?",
                        Options = new[] { "Paris", "London", "Berlin" },
                        CorrectAnswerIndex = 0,
                        Timeout = 30
                    },
                    new TrueFalseQuestion
                    {
                        Title = "The sky is blue.",
                        IsTrue = true,
                        Timeout = 15
                    }
                }
            };
            var outputJson = JsonSerializer.Serialize(questionSet, options);
            // Output
            Console.WriteLine("Deserialized and re-serialized JSON:\n" + outputJson);


            // Validate that each question has a questionType property
            using var doc = JsonDocument.Parse(outputJson);
            var questions = doc.RootElement.GetProperty("questions");
            foreach (var question in questions.EnumerateArray())
            {
                Assert.True(question.TryGetProperty("questionType", out _),
                    $"A question is missing the 'questionType' property: {question}");
            }
        }
    }
}
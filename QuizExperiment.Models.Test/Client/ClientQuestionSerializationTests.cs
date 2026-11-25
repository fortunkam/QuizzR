using QuizExperiment.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizExperiment.Models.Test
{
    public class ClientQuestionSerializationTests
    {
        [Fact]
        public void Serialize_ClientQuestion_Sets_QuestionType()
        {
            // Arrange
            var question = new ClientMultipleChoiceQuestion
            {
                Title = "Sample Question",
                ImageUrl = "https://example.com/image.png",
                Options = new List<string> { "Option 1", "Option 2", "Option 3" }
            };

            // Serialize as base type to ensure questionType is included
            var outputJson = JsonSerializer.Serialize<ClientQuestion>(question);

            // Output
            Console.WriteLine("Deserialized and re-serialized JSON:\n" + outputJson);

            using var doc = JsonDocument.Parse(outputJson);

            Assert.True(doc.RootElement.TryGetProperty("questionType", out _),
                    $"A question is missing the 'questionType' property: {question}");
        }

        [Fact]
        public void Serialize_GuessTheNumberQuestion_Sets_QuestionType()
        {
            // Arrange
            var question = new ClientGuessTheNumberQuestion
            {
                Title = "Guess the number",
                ImageUrl = "https://example.com/image.png",
                MinValue = 1,
                MaxValue = 100
            };

            // Serialize as base type to ensure questionType is included
            var outputJson = JsonSerializer.Serialize<ClientQuestion>(question);

            // Output
            Console.WriteLine("Serialized JSON:\n" + outputJson);

            using var doc = JsonDocument.Parse(outputJson);

            Assert.True(doc.RootElement.TryGetProperty("questionType", out var typeProperty),
                    $"A question is missing the 'questionType' property: {question}");
            Assert.Equal("guessTheNumber", typeProperty.GetString());
        }

        [Fact]
        public void SayWhatYouSeeStringEqualityComparer_IsCaseInsensitive()
        {
            // Arrange
            var comparer = new SayWhatYouSeeStringEqualityComparer();
            var answer1 = "Hello World";
            var answer2 = "hello world";

            // Act
            var areEqual = comparer.Equals(answer1, answer2);

            // Assert
            Assert.True(areEqual, "SayWhatYouSeeStringEqualityComparer should treat answers as equal regardless of case.");
        }

        [Fact]
        public void ClientSayWhatYouSeeAnswer_Validation()
        {

            var answer1 = new ClientSayWhatYouSeeAnswer
            {
                Answers = new List<string>
                {
                    "one"
                }
            };

            var answer2 = new ClientSayWhatYouSeeAnswer
            {
                Answers = new List<string>
                {
                    "One",
                    "Two"
                }
            };

            Assert.True(answer1 == answer2);
        }
    }
}

using QuizExperiment.Models.Client;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class GuessTheNumberQuestion : Question
    {
        [JsonPropertyName("correctAnswer")]
        public int CorrectAnswer { get; set; }

        [JsonPropertyName("minValue")]
        public int MinValue { get; set; }

        [JsonPropertyName("maxValue")]
        public int MaxValue { get; set; }

        [JsonIgnore]
        public override bool IsValid =>
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(ImageUrl) &&
            MinValue < MaxValue &&
            CorrectAnswer >= MinValue &&
            CorrectAnswer <= MaxValue;

        public override ClientAnswer GetCorrectAnswer()
        {
            return new ClientGuessTheNumberAnswer
            {
                Answer = CorrectAnswer
            };
        }

        public override ClientQuestion ToClientQuestion()
        {
            return new ClientGuessTheNumberQuestion
            {
                Title = Title,
                ImageUrl = ImageUrl,
                MinValue = MinValue,
                MaxValue = MaxValue
            };
        }
    }
}

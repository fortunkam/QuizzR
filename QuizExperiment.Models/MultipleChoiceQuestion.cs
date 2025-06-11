using QuizExperiment.Models.Client;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class MultipleChoiceQuestion : Question
    {
        [JsonPropertyName("options")]
        public string[]? Options { get; set; }

        [JsonPropertyName("correctAnswerIndex")]
        public int CorrectAnswerIndex { get; set; }

       

        [JsonIgnore]
        public override bool IsValid =>
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(ImageUrl) &&
            Options != null &&
            Options.Length == 4 &&
            Options.All(o => !string.IsNullOrWhiteSpace(o)) &&
            CorrectAnswerIndex >= 0 &&
            CorrectAnswerIndex < 4;

        public override ClientAnswer GetCorrectAnswer()
        {
            return new ClientMultipleChoiceAnswer
            {
                AnswerIndex = CorrectAnswerIndex
            };
        }

        public override ClientQuestion ToClientQuestion()
        {
            return new ClientMultipleChoiceQuestion
            {
                Title = Title,
                ImageUrl = ImageUrl,
                Options = Options?.ToList() ?? new List<string>()
            };
        }

    }
}

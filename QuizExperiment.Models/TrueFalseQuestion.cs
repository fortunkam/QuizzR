using QuizExperiment.Models.Client;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class TrueFalseQuestion : Question
    {
        [JsonPropertyName("isTrue")]
        public bool IsTrue { get; set; }

        [JsonIgnore]
        public override bool IsValid =>
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(ImageUrl);

        public override ClientAnswer GetCorrectAnswer()
        {
            return new ClientTrueFalseAnswer
            {
                Answer = IsTrue
            };
        }

        public override ClientQuestion ToClientQuestion()
        {
            return new ClientTrueFalseQuestion
            {
                Title = Title,
                ImageUrl = ImageUrl
            };
        }
    }
}

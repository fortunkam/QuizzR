using System.Text.Json.Serialization;

namespace QuizExperiment.Models.Client
{
    public class ClientMultipleChoiceQuestion : ClientQuestion
    {
        public ClientMultipleChoiceQuestion() : base() { }

        [JsonPropertyName("options")]
        public List<string> Options { get; set; } = new List<string>();

    }
}
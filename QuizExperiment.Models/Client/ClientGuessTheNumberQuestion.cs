using System.Text.Json.Serialization;

namespace QuizExperiment.Models.Client
{
    public class ClientGuessTheNumberQuestion : ClientQuestion
    {
        public ClientGuessTheNumberQuestion() : base() { }

        [JsonPropertyName("minValue")]
        public int MinValue { get; set; }

        [JsonPropertyName("maxValue")]
        public int MaxValue { get; set; }
    }
}

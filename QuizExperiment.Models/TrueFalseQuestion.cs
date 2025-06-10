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
    }
}

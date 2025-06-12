using QuizExperiment.Models.Client;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "questionType")]
    [JsonDerivedType(typeof(MultipleChoiceQuestion), "multipleChoice")]
    [JsonDerivedType(typeof(TrueFalseQuestion), "trueFalse")]
    public abstract class Question
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }

        [JsonIgnore]
        public abstract bool IsValid { get; }

        public abstract ClientAnswer GetCorrectAnswer();

        public abstract ClientQuestion ToClientQuestion();
    }
}

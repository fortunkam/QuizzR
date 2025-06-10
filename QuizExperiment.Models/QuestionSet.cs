using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class QuestionSet
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("userid")]
        public string? UserId { get; set; }

        [JsonPropertyName("questions")]
        [JsonConverter(typeof(PolymorphicQuestionListConverter))]
        public List<Question>? Questions { get; set; }

        [JsonPropertyName("folderPath")]
        public string? FolderPath { get; set; }
    }

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
    }

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
    }

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

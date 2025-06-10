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
}

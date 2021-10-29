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
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("questions")]
        public Question[] Questions { get; set; }
    }


    public class Question
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }

        [JsonPropertyName("options")]
        public string[] Options { get; set; }

        [JsonPropertyName("correctAnswers")]
        public string[] CorrectAnswers { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models
{
    public class QuestionSetSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userid")]
        public string UserId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTimeOffset LastModified { get; set; }

    }
}

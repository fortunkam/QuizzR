using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizExperiment.Models.Client
{
    /// <summary>
    /// Base question detail to be shown to the client (participant)
    /// </summary>
    [JsonDerivedType(typeof(ClientMultipleChoiceQuestion), "multipleChoice")]
    [JsonDerivedType(typeof(ClientTrueFalseQuestion), "trueFalse")]
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "questionType")]
    public abstract class ClientQuestion
    {
        public ClientQuestion() { }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }
    }
}

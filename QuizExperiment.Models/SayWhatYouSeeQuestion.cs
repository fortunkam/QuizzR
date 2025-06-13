using QuizExperiment.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizExperiment.Models
{
    public class SayWhatYouSeeQuestion : Question
    {
        public List<string> Answers { get; set; } = new List<string>();

        public override ClientAnswer GetCorrectAnswer()
        {
            return new ClientSayWhatYouSeeAnswer { Answers = this.Answers  };
        }
        public override ClientQuestion ToClientQuestion()
        {
            return new ClientSayWhatYouSeeQuestion
            {
                Title = "Say What You See",
                ImageUrl = ImageUrl,
            };
        }

        [JsonIgnore]
        public override bool IsValid =>
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(ImageUrl) &&
            Answers != null && Answers.Any(r => !string.IsNullOrWhiteSpace(r));
    }
}

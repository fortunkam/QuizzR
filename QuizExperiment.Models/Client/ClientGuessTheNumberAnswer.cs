using System;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models.Client
{
    public class ClientGuessTheNumberAnswer : ClientAnswer, IEquatable<ClientGuessTheNumberAnswer>
    {
        [JsonPropertyName("answer")]
        public int Answer { get; set; }

        public override (string description, string index, string buttonName) GetAnswerDetails(ClientQuestion question)
        {
            return question switch
            {
                ClientGuessTheNumberQuestion _ => (Answer.ToString(), Answer.ToString(), Answer.ToString()),
                _ => throw new InvalidOperationException("Invalid question type for guess the number answer.")
            };
        }

        public override bool Equals(ClientAnswer? other)
        {
            return Equals(other as ClientGuessTheNumberAnswer);
        }

        public bool Equals(ClientGuessTheNumberAnswer? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Answer == other.Answer;
        }

        public override int GetHashCode()
        {
            return Answer.GetHashCode();
        }
    }
}

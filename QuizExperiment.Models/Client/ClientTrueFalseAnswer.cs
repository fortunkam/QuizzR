using System;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models.Client
{
    public class ClientTrueFalseAnswer : ClientAnswer, IEquatable<ClientTrueFalseAnswer>
    {
        [JsonPropertyName("answer")]
        public bool Answer { get; set; }

        public override (string description, int index, string buttonName) GetAnswerDetails(Question question)
        {
            var answerText = Answer ? "True" : "False";
            return (answerText, Answer ? 1 : 0, answerText);
        }

        public override bool Equals(ClientAnswer? other)
        {
            return Equals(other as ClientTrueFalseAnswer);
        }

        public bool Equals(ClientTrueFalseAnswer? other)
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
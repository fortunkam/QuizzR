using System;
using System.Text.Json.Serialization;

namespace QuizExperiment.Models.Client
{
    public class ClientMultipleChoiceAnswer : ClientAnswer, IEquatable<ClientMultipleChoiceAnswer>
    {
        [JsonPropertyName("answerIndex")]
        public int AnswerIndex { get; set; }

        public override (string description, int index, string buttonName) GetAnswerDetails(Question question)
        {
            return question switch
            {
                MultipleChoiceQuestion mcq => (mcq.Options[AnswerIndex], AnswerIndex, AnswerIndex.GetButtonNameFromIndex()),
                _ => throw new InvalidOperationException("Invalid question type for multiple choice answer.")
            };
        }

        public override bool Equals(ClientAnswer? other)
        {
            return Equals(other as ClientMultipleChoiceAnswer);
        }

        public bool Equals(ClientMultipleChoiceAnswer? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return AnswerIndex == other.AnswerIndex;
        }

        public override int GetHashCode()
        {
            return AnswerIndex.GetHashCode();
        }
    }
}
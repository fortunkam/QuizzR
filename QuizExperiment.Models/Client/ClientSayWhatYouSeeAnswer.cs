using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace QuizExperiment.Models.Client
{
    public class ClientSayWhatYouSeeAnswer : ClientAnswer, IEquatable<ClientSayWhatYouSeeAnswer>
    {
        [JsonPropertyName("answers")]
        public List<string> Answers { get; set; } = new List<string>();

        public override (string description, string index, string buttonName) GetAnswerDetails(ClientQuestion question)
        {
            var description = Answers[0];

            if(Answers.Count > 1)
            {
                var otherAnswers = string.Join(" or ", Answers.Skip(1).Select(a => $"\"{a.ToLowerInvariant()}\""));
                description += $" (also allowed {otherAnswers})";
            }

            return (description, "0", "");
        }

        public override bool Equals(ClientAnswer? other)
        {
            return Equals(other as ClientSayWhatYouSeeAnswer);
        }

        public bool Equals(ClientSayWhatYouSeeAnswer? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            // Return true if any item in either list matches any item in the other list (case-insensitive)
            var comparer = new SayWhatYouSeeStringEqualityComparer();
            return Answers.Any(a => other.Answers.Contains(a, comparer)) ||
                   other.Answers.Any(a => Answers.Contains(a, comparer));
        }

        public static bool operator ==(ClientSayWhatYouSeeAnswer? left, ClientSayWhatYouSeeAnswer? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ClientSayWhatYouSeeAnswer? left, ClientSayWhatYouSeeAnswer? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            // Use case-insensitive hash code
            return Answers
                .Select(a => a?.ToLowerInvariant().GetHashCode() ?? 0)
                .Aggregate(0, (current, hashCode) => current ^ hashCode);
        }
    }
}
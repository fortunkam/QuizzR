using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizExperiment.Models.Client
{
    [JsonDerivedType(typeof(ClientMultipleChoiceAnswer), "multipleChoice")]
    [JsonDerivedType(typeof(ClientTrueFalseAnswer), "trueFalse")]
    [JsonDerivedType(typeof(ClientSayWhatYouSeeAnswer), "sayWhatYouSee")]
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "anwserType")]
    public abstract class ClientAnswer : IEquatable<ClientAnswer>
    {
        public abstract (string description, string index, string buttonName) GetAnswerDetails(ClientQuestion question);

        public abstract bool Equals(ClientAnswer? other);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as ClientAnswer);
        }

        public static bool operator ==(ClientAnswer? left, ClientAnswer? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(ClientAnswer? left, ClientAnswer? right)
        {
            return !(left == right);
        }

        public abstract override int GetHashCode();
    }
}

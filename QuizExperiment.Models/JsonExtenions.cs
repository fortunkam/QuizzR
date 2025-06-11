using QuizExperiment.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace QuizExperiment.Models
{
    public static class JsonExtensions
    {
        public static void AddNativePolymorphicTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            Type baseValueObjectType = typeof(ClientQuestion);
            if (baseValueObjectType.IsAssignableFrom(jsonTypeInfo.Type))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "questionType",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                };
                var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(ClientQuestion))); // Fixed MyBaseClass => ValueObject
                foreach (var t in types.Select(t => new JsonDerivedType(t, t.Name.ToLowerInvariant()))) // Fixed ToLower() => ToLowerInvariant
                    jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(t);
            }
        }
    }
}

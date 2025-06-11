using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace QuizExperiment.Models
{
    public class InheritedPolymorphismResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);

            // Only handles class hierarchies -- interface hierarchies left out intentionally here
            if (!type.IsSealed && typeInfo.PolymorphismOptions is null && type.BaseType != null)
            {
                // recursively resolve metadata for the base type and extract any derived type declarations that overlap with the current type
                JsonPolymorphismOptions? basePolymorphismOptions = GetTypeInfo(type.BaseType, options).PolymorphismOptions;
                if (basePolymorphismOptions != null)
                {
                    foreach (JsonDerivedType derivedType in basePolymorphismOptions.DerivedTypes)
                    {
                        if (type.IsAssignableFrom(derivedType.DerivedType))
                        {
                            typeInfo.PolymorphismOptions ??= new()
                            {
                                IgnoreUnrecognizedTypeDiscriminators = basePolymorphismOptions.IgnoreUnrecognizedTypeDiscriminators,
                                TypeDiscriminatorPropertyName = basePolymorphismOptions.TypeDiscriminatorPropertyName,
                                UnknownDerivedTypeHandling = basePolymorphismOptions.UnknownDerivedTypeHandling,
                            };

                            typeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
                        }
                    }
                }
            }

            return typeInfo;
        }
    }
}

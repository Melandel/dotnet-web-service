using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

abstract class TypeInvolvingConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static TypeInvolvingConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		var t when t.IsANonGenericImplementationOfIConstrainedCollection(out var instantiationOperationParameterTypeCandidates) => NonGenericConstrainedCollectionReadingOperation.Instance(type, instantiationOperationParameterTypeCandidates),
		var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                            => PositionalRecordReadingOperation.Instance(type, propertyBasedConstructor!),
		var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                           => ClassWithPublicConstructorHavingParametersReadingOperation.Instance(type, constructorsWithParameters),
		var t when t.IsClassWithPublicParameterlessConstructorAndPublicSetters(out var publicSettableProperties)                => ClassWithPublicParameterlessConstructorAndPublicSettersReadingOperation.Instance(type, publicSettableProperties),
		var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                         => ClassWithPublicStaticFactoryMethodReadingOperation.Instance(type, staticFactoryMethods),
		var t when t.IsInterface => InterfaceReadingOperation.Instance(type),
		_ => throw new InvalidOperationException($"{nameof(TypeInvolvingConstrainedTypeReadingOperation)}: Cannot resolve an {nameof(TypeInvolvingConstrainedTypeReadingOperation)} for type {type.GetName()}.")
	};

	protected object?[] ReadParameterValuesFor(Type targetType, MethodBase instantiator, ref Utf8JsonReader reader, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instantiatorParameterInfos = instantiator.GetParameters().ToList();
		var instantiatorParameterValues = new object?[instantiatorParameterInfos.Count];
		var missingDeserializedParameters = new List<ParameterInfo>(instantiatorParameterInfos);

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}

			var propertyName = reader.GetString();
			var matchingParameterIndex = instantiatorParameterInfos.FindIndex(pn => pn.Name.Matches(propertyName));

			reader.Read();
			var propertyNameNotRecognized = matchingParameterIndex == -1;
			if (propertyNameNotRecognized)
			{
				continue;
			}

			var matchingParameterType = instantiatorParameterInfos[matchingParameterIndex].ParameterType;
			var deserializedParameter =
				For(matchingParameterType)
					.Execute(ref reader, matchingParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			instantiatorParameterValues[matchingParameterIndex] = deserializedParameter;
			missingDeserializedParameters.RemoveAll(p => p.Name.Matches(propertyName));
		}

		if (missingDeserializedParameters.Count > 0)
		{
			var nullabilityContext = new NullabilityInfoContext();
			for (int i = 0; i < missingDeserializedParameters.Count; i++)
			{
				var missingParameter = missingDeserializedParameters[i];
				var isNullable = NullabilityInfo.NullabilityByConstructorOrMethodParameter
					.GetOrAdd(instantiator, _ => new ConcurrentDictionary<ParameterInfo, bool>())
					.GetOrAdd(missingParameter, _ => GetNullabilityInfoFrom(missingParameter, nullabilityContext));

				if (isNullable)
				{
					var missingParameterIndex = instantiatorParameterInfos.FindIndex(p => p.Name == missingParameter.Name);
					instantiatorParameterValues[missingParameterIndex] = null;
					missingDeserializedParameters.Remove(missingParameter);
				}
			}
		}

		if (missingDeserializedParameters.Count != 0)
		{
			throw new InvalidOperationException($"Cannot deserialize {targetType.GetName()} : parameter(s) {string.Join(',', missingDeserializedParameters)} are missing.");
		}

		return instantiatorParameterValues;
	}
	bool GetNullabilityInfoFrom(ParameterInfo missingParameter, NullabilityInfoContext nullabilityContext)
	=> nullabilityContext.Create(missingParameter).ReadState switch
	{
		NullabilityState.Nullable => true,
		NullabilityState.Unknown => false,
		_ => false
	};
}

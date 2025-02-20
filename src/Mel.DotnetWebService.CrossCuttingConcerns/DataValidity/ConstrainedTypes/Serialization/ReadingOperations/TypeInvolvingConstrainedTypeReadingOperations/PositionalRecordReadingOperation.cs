using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class PositionalRecordReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly PositionalRecordReadingOperation _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = new Dictionary<Type, ConstructorInfo>();
	PositionalRecordReadingOperation()
	{
	}
	public static PositionalRecordReadingOperation Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead");
		}

		var propertyBasedConstructor = PropertyBasedConstructorsByPositionalRecordType[targetType];
		var constructorParameterInfos = propertyBasedConstructor.GetParameters().ToList();

		var constructorParameterValues = new object?[constructorParameterInfos.Count];
		var missingDeserializedParameterNames = constructorParameterInfos.Select(p => p.Name).ToList();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}

			var propertyName = reader.GetString();
			var matchingParameterIndex = -1;
			matchingParameterIndex = constructorParameterInfos.FindIndex(pn => pn.Name.Matches(propertyName));

			reader.Read();
			var matchingParameterType = constructorParameterInfos[matchingParameterIndex].ParameterType;
			var deserializedParameter =
				For(matchingParameterType)
					.Execute(ref reader, matchingParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			constructorParameterValues[matchingParameterIndex] = deserializedParameter;
			missingDeserializedParameterNames.RemoveAll(parameterName => parameterName.Matches(propertyName));
		}

		if (missingDeserializedParameterNames.Count != 0)
		{
			throw new InvalidOperationException($"Cannot deserialize {targetType.GetName()} : parameter(s) {string.Join(',', missingDeserializedParameterNames)} are missing.");
		}

		var deserialized =  propertyBasedConstructor?.Invoke(constructorParameterValues);

		if (reader.TokenType != JsonTokenType.EndObject)
		{
			throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead");
		}

		return deserialized;
	}
}

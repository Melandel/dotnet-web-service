using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class PositionalRecordReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly PositionalRecordReadingOperation _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = new Dictionary<Type, ConstructorInfo>();
	PositionalRecordReadingOperation()
	{
	}
	public static PositionalRecordReadingOperation Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		if (!PropertyBasedConstructorsByPositionalRecordType.ContainsKey(targetType))
		{
			PropertyBasedConstructorsByPositionalRecordType.Add(targetType, propertyBasedConstructor);
		}
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		_ = reader.TokenType switch
		{
			JsonTokenType.StartObject => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead")
		};

		var propertyBasedConstructor = PropertyBasedConstructorsByPositionalRecordType[targetType];
		var constructorParameterInfos = propertyBasedConstructor.GetParameters().ToList();

		var constructorParameterValues = new object?[constructorParameterInfos.Count];
		foreach (var param in propertyBasedConstructor!.GetParameters())
		{
			var matchingParameterIndex = -1;
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				var propertyName = reader.GetString();
				matchingParameterIndex = constructorParameterInfos.FindIndex(pn => pn.Name.Matches(propertyName));
				reader.Read();
			}

			var matchingParameterType = constructorParameterInfos[matchingParameterIndex].ParameterType;
			var deserializedParameter =
				For(matchingParameterType)
					.Execute(ref reader, matchingParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			constructorParameterValues[matchingParameterIndex] = deserializedParameter;
		}

		var deserialized =  propertyBasedConstructor?.Invoke(constructorParameterValues);

		_ = reader.TokenType switch
		{
			JsonTokenType.EndObject => reader.Read(),
			_ => throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead")
		};

		return deserialized;
	}
}

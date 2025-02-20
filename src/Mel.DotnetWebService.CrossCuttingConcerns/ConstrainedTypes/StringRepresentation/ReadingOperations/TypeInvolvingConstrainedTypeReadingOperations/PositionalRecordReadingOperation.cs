using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

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
		var constructorParameters = new List<object?>();

		foreach (var param in propertyBasedConstructor!.GetParameters())
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				reader.Read();
			}

			var deserializedParameter = ConstrainedTypeConverterReadingOperation
				.For(param.ParameterType)
				.Execute(ref reader, param.ParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			constructorParameters.Add(deserializedParameter);
		}

		var deserialized =  propertyBasedConstructor?.Invoke(constructorParameters.ToArray());

		_ = reader.TokenType switch
		{
			JsonTokenType.EndObject => reader.Read(),
			_ => throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead")
		};

		return deserialized;
	}
}

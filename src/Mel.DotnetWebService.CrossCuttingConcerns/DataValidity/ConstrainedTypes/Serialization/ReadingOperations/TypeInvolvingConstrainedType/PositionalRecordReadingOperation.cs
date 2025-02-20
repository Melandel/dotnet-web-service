using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class PositionalRecordReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly PositionalRecordReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = new ConcurrentDictionary<Type, ConstructorInfo>();
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
		var parameters = ReadParameterValuesFor(targetType, propertyBasedConstructor, ref reader, options, preComputedOptionsWithoutConstrainedTypeConverter);

		var deserialized =  propertyBasedConstructor?.Invoke(parameters);

		if (reader.TokenType != JsonTokenType.EndObject)
		{
			throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead");
		}

		return deserialized;
	}
}

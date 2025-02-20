using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

class ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation Instance = new();
	ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var nestedValue = ((ObjectTypeSpecificallyGeneratedForSerializationPurposes)value).Value;
		var serialized = JsonSerializer.Serialize(nestedValue, options);
		writer.WriteRawValue(serialized);
	}
}

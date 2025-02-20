using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

class ConstrainedTypesIgnoringWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly ConstrainedTypesIgnoringWritingOperation Instance = new();
	ConstrainedTypesIgnoringWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var opt = options.Without<ConstrainedTypeJsonConverter>();
		writer.WriteRawValue(JsonSerializer.Serialize(value, opt));
	}
}

using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class ConstrainedDataTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly ConstrainedDataTypeReadingOperation Instance = new();
	ConstrainedDataTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		var valueType = targetType.GetConstrainedTypeRootType();
		var valueFoundInsideJson = reader.GetNativelySupportedValue(valueType);
		return targetType.ReconstituteFromValueFoundInsideJson(valueFoundInsideJson);
	}
}

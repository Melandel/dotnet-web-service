using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class ConstrainedDataTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly ConstrainedDataTypeReadingOperation Instance = new();
	ConstrainedDataTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		return targetType.DeserializeFromJson(targetType.GetConstrainedTypeRootType() switch
		{
			var rootType when rootType == typeof(Guid) => reader.GetString(),
			var rootType when rootType == typeof(Guid) => reader.GetString(),
			var rootType => throw new NotImplementedException($"type {rootType} not supported")
		});
		//var rootType = targetType.GetConstrainedTypeRootType();
		//if (rootType == typeof(Guid))
		//{
		//	var v = reader.GetString()!;
		//	var w = Guid.Parse(v);
		//	return NonEmptyGuid.FromGuid(w);
		//}
		//return rootType switch
		//{
		//	var t when t == typeof(Guid) => Guid.Parse(reader.GetString()!),
		//	var t when t == typeof(string) => reader.GetString()!,
		//	_ => throw new NotImplementedException($"type {rootType} not supported")
		//};
	}
}

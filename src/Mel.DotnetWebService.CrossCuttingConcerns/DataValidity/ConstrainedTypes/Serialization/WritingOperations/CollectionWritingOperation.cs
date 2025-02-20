using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

class CollectionWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly CollectionWritingOperation Instance = new();
	CollectionWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();

		var valueAsNonGenericIEnumerable = ConstrainedTypeInfos.TryGet(value.GetType(), out var constrainedTypeInfo)
			? (System.Collections.IEnumerable) constrainedTypeInfo.InvokeImplicitConversionToRootType(value)
			: (System.Collections.IEnumerable) value;
		foreach (var item in valueAsNonGenericIEnumerable)
		{
			var serialized = JsonSerializer.Serialize(item, options);
			writer.WriteRawValue(serialized);
		}

		writer.WriteEndArray();
	}
}

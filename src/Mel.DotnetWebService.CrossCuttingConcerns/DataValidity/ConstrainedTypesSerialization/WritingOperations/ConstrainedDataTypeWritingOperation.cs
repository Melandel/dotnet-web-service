using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

class ConstrainedDataTypeWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly ConstrainedDataTypeWritingOperation Instance = new();
	ConstrainedDataTypeWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		foreach (var converter in value.GetType().GetUserDefinedConversions(browseParentTypes: true))
		{
			try
			{
				var converted = converter.Invoke(null, new[] { value });
				var serialized = JsonSerializer.Serialize(converted, options);
				writer.WriteRawValue(serialized);
				return;
			}
			catch
			{
			}
		}
	}
}

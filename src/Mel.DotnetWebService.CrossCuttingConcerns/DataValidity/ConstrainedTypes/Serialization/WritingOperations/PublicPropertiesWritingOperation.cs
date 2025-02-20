using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

class PublicPropertiesWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly PublicPropertiesWritingOperation Instance = new();
	PublicPropertiesWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var type = value.GetType();
		var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		writer.WriteStartObject();
		foreach (var prop in properties)
		{
			var propName = prop.Name;
			writer.WritePropertyName(propName);

			var propValue = prop.GetValue(value, Array.Empty<object>());
			var serializedPropValue = JsonSerializer.Serialize(propValue, options);
			writer.WriteRawValue(serializedPropValue);
		}
		writer.WriteEndObject();
	}
}

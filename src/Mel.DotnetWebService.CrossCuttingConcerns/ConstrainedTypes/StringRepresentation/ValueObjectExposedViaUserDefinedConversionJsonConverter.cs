using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public class ValueObjectExposedViaUserDefinedConversionJsonConverter : JsonConverter<object>
{
	readonly static JsonConverter<object> DefaultConverter = (JsonConverter<object>) JsonSerializerOptions.Default.GetConverter(typeof(object));

	public override bool CanConvert(Type typeToConvert)
	=> typeToConvert.DefinesAValueObject();

	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	=> DefaultConverter.Read(ref reader, typeToConvert, options);

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		foreach (var converter in value.GetType().GetUserDefinedConversions(includeParentTypes: true))
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

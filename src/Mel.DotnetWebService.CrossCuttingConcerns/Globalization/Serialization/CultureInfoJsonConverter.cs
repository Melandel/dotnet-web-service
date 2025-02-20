using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;

public sealed class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
	public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
		{
			throw new InvalidOperationException($"{typeToConvert.FullName} : {GetType().Name} must {nameof(Read)} on {JsonTokenType.String} json token, but is being called on {reader.TokenType} json token instead");
		}

		var name = reader.GetString();
		try
		{
			var culture = CultureInfo.GetCultureInfo(name!);
			return culture;
		}
		catch (CultureNotFoundException ex)
		{
			throw new JsonException($"Invalid culture name: '{name}'.", ex);
		}
	}

	public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
	=> writer.WriteStringValue(value.Name);
}

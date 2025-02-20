using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;

public sealed class RegionInfoJsonConverter : JsonConverter<RegionInfo>
{
	public override RegionInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
		{
			throw new InvalidOperationException($"{typeToConvert.FullName} : {GetType().Name} must {nameof(Read)} on {JsonTokenType.String} json token, but is being called on {reader.TokenType} json token instead");
		}

		
		var name = reader.GetString();
		try
		{
			return new RegionInfo(name!);
		}
		catch (ArgumentException ex)
		{
			throw new JsonException($"Invalid region name: '{name}'.", ex);
		}
	}

	public override void Write(
		Utf8JsonWriter writer,
		RegionInfo value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.Name);
	}
}

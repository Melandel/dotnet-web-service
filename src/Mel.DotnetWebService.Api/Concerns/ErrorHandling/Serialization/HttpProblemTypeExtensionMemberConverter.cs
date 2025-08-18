﻿using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Serialization;
public class HttpProblemTypeExtensionMemberConverter : JsonConverter<IEnumerable<HttpProblemTypeExtensionMember>>
{
	public override IEnumerable<HttpProblemTypeExtensionMember> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	=> throw new NotImplementedException("Unnecessary because Read is never supposed to be used.");

	public override void Write(Utf8JsonWriter writer, IEnumerable<HttpProblemTypeExtensionMember> httpProblemTypeExtensionMemberValue, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var value in httpProblemTypeExtensionMemberValue)
		{
			writer.WriteStringValue(value.ToString());
		}
		writer.WriteEndArray();
	}
}

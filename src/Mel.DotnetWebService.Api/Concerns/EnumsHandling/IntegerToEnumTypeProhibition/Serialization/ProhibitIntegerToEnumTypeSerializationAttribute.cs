using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.IntegerToEnumTypeProhibition.Serialization;

class ProhibitIntegerToEnumTypeSerializationAttribute : IConfigureNamedOptions<JsonOptions>
{
	public void Configure(string? name, JsonOptions options) => Configure(options);
	public void Configure(JsonOptions options)
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
	}
}


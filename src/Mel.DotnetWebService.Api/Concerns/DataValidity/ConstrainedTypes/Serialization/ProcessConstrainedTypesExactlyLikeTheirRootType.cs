using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.Serialization;

public class ProcessConstrainedTypesExactlyLikeTheirRootType :  IConfigureNamedOptions<JsonOptions>
{
	public void Configure(string? name, JsonOptions options)
	=> Configure(options);

	public void Configure(JsonOptions options)
	{
		options.JsonSerializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
	}
}

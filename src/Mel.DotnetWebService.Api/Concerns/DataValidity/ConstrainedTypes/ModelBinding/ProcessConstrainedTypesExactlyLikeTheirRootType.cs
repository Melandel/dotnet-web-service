using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.ModelBinding;

public class ProcessConstrainedTypesExactlyLikeTheirRootType :  IConfigureNamedOptions<MvcOptions>
{
	public void Configure(string? name, MvcOptions options)
	=> Configure(options);

	public void Configure(MvcOptions options)
	{
		options.ModelBinderProviders.Insert(0, new ConstrainedTypeBinderProvider());
	}
}

using Microsoft.Extensions.DependencyInjection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddController<TController>(this IServiceCollection services)
		where TController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		if (services is null)
			return null;

		var partManager = (Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager)services
			.Last(descriptor => descriptor.ServiceType == typeof(Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager))
			.ImplementationInstance;

		partManager.FeatureProviders.Add(ApplicationFeatureProviderTestDouble.That_Adds_Controller<TController>());

		return services;
	}
}

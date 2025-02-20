using System.Runtime.CompilerServices;
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

	public static IServiceCollection AssertThatServiceIsNotInjected(
		this IServiceCollection services,
		Type typeOfServiceThatShouldNotBeInjected,
		string testThatRequiresTheAbsenceOfTheService,
		[CallerFilePath] string callerFilePath = "",
		[CallerMemberName] string callerMethodeName = "")
	=> services switch
	{
		null => null,
		_ when services.Any(serviceDescriptor => serviceDescriptor.ImplementationType == typeOfServiceThatShouldNotBeInjected) => throw TestDataIntegrityException.GeneratedBy(Path.GetFileNameWithoutExtension(callerFilePath), callerMethodeName, $"the service \"{typeOfServiceThatShouldNotBeInjected.FullName}\" must not be injected in order for test \"{testThatRequiresTheAbsenceOfTheService}()\" to be functional"),
		_ => services
	};
}

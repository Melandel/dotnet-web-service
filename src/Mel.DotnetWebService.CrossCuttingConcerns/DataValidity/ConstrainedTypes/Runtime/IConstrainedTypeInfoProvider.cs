using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public interface IConstrainedTypeInfoProvider
{
	bool TryGet(Type type, out ConstrainedTypeInfo constrainedType);
}

public static class ExtensionMethods
{
	public static IServiceCollection AddConstrainedTypes(this IServiceCollection services)
	{
		var fullScopeAssembly = Assembly.GetEntryAssembly() switch
		{
			null => Assembly.GetCallingAssembly(),
			var entryAssembly when entryAssembly.FullName!.Contains("testhost") => Assembly.GetCallingAssembly(),
			var entryAssembly => entryAssembly
		};
		var constrainedTypeProvider = ConstrainedTypeInfoProvider.LoadConstrainedTypesDeclaredIn(fullScopeAssembly);

		services.AddSingleton<IConstrainedTypeInfoProvider>(constrainedTypeProvider);
		return services;
	}
}

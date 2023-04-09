using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class Types
{
	public static IReadOnlyCollection<Type> AllControllerTypesDefinedByOurOrganization
	{
		get
		{
			var types = _all_known_assemblies.Value
				.SelectMany(asmb => asmb
					.GetTypes()
					.SelectMany(t => new [] { t }.Concat(t.GetNestedTypes()))
					.Where(t => t.IsDefinedByOurOrganization() && t.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))))
				.Distinct()
				.ToArray();

			return types switch
			{
				[_,..] => types,
				_ => throw TestDataIntegrityException.GeneratedBy(typeof(Types), nameof(Types.AllControllerTypesDefinedByOurOrganization), "Found no type matching this criteria")
			};
		}
	}

	static readonly Lazy<IReadOnlyCollection<Assembly>> _all_known_assemblies = new(
		() => AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(loadedAssembly => loadedAssembly.GetReferencedAssemblies())
			.Distinct()
			.Select(referencedAssemblyName =>
			{
				Assembly loadedAssembly = null;
				try { loadedAssembly = Assembly.Load(referencedAssemblyName); }
				catch { }
				return loadedAssembly;
			})
			.Where(loadedAssembly => loadedAssembly != null)
			.ToArray()
	);
}


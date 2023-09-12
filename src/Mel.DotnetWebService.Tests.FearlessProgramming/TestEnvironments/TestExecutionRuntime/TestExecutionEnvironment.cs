using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestExecutionRuntime;

static class TestExecutionEnvironment
{
	public static IReadOnlyCollection<Type> All_Types_Defined_By_Our_Organization
	=> _all_types_defined_by_our_organization.Value;

	static readonly Lazy<IReadOnlyCollection<Type>> _all_types_defined_by_our_organization = new(() =>
		TestExecutionEnvironment.All_Known_Assemblies
			.SelectMany(assembly =>
				assembly
					.GetTypes()
					.SelectMany(t => new [] { t }.Concat(t.GetNestedTypes()))
					.Where(t => t.IsDefinedByOurOrganization()))
			.Distinct()
			.ToArray());

	public static IReadOnlyCollection<Assembly> All_Known_Assemblies
	=> _all_known_assemblies.Value;

	static readonly Lazy<IReadOnlyCollection<Assembly>> _all_known_assemblies = new(() =>
		AppDomain.CurrentDomain.GetAssemblies()
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

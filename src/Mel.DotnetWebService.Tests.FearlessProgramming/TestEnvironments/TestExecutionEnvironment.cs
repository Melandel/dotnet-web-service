using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments;

static class TestExecutionEnvironment
{
	public static IReadOnlyCollection<Assembly> All_Known_Assemblies
	=> _all_known_assemblies.Value;

	static readonly Lazy<IReadOnlyCollection<Assembly>> _all_known_assemblies = new(
		() => AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(loadedAssembly => loadedAssembly.GetReferencedAssemblies())
			.Distinct()
			.Select(referencedAssemblyName => Assembly.Load(referencedAssemblyName))
			.ToArray()
	);
}

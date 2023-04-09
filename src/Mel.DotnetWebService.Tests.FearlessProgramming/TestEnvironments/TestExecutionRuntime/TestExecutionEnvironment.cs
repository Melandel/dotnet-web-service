using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestExecutionRuntime;

static class TestExecutionEnvironment
{
	public static IReadOnlyCollection<Assembly> All_Known_Assemblies
	=> _all_known_assemblies.Value;

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


using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class Types
{
	public static IReadOnlyCollection<Type> AllConcreteConstrainedTypes
	=> _allConcreteConstrainedTypes.Value;

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

	static readonly Lazy<IReadOnlyCollection<Type>> _allConcreteConstrainedTypes = new(
		() =>
		{
			ConstrainedTypesLoader.LoadConstrainedTypesFromExecutingAssemblyIfNecessary();

			var types = _all_known_assemblies.Value
				.Where(asmb => !asmb.FullName.StartsWith("System") && !asmb.FullName.StartsWith("Microsoft") && !asmb.FullName.StartsWith("netstandard"))
				.Distinct()
				.SelectMany(asmb =>
				{
						var v = asmb
							.GetTypes()
							.SelectMany(t => new[] { t }.Concat(t.GetNestedTypes()))
							.Where(t => !t.IsAbstract && !t.IsGenericType && ConstrainedTypeInfos.Include(t));
					return v;
					})
				.Distinct()
				.ToNonEmptyHashSet();

			return types;
		});

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


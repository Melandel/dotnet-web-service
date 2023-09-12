using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class Types
{
	public static IReadOnlyCollection<Type> AllControllerTypesDefinedByOurOrganization
	=> _allControllerTypesDefinedByOurOrganization switch
	{
		[_,..] types => types,
		_ => throw TestDataIntegrityException.GeneratedBy(typeof(Types), nameof(Types.AllControllerTypesDefinedByOurOrganization), "Found no type matching this criteria")
	};

	public static IReadOnlyCollection<Type> AllEnumTypesDefinedByOurOrganization
	=> _allEnumTypesDefinedByOurOrganization switch
	{
		[_, ..] enumTypes => enumTypes,
		_ => throw TestDataIntegrityException.GeneratedBy(typeof(Types), nameof(Types.AllEnumTypesDefinedByOurOrganization), "Found no type matching this criteria")
	};

	public static IReadOnlyCollection<Type> AllConcreteConstrainedTypes
	=> _allConcreteConstrainedTypes.Value;

	static Type[] _allControllerTypesDefinedByOurOrganization
	=> _all_types_defined_by_our_organization.Value
		.Where(t => t.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.ControllerBase)))
		.ToArray();

	static Type[] _allEnumTypesDefinedByOurOrganization
	=> _all_types_defined_by_our_organization.Value
		.Where(t => t.IsEnum)
		.ToArray();

	static readonly Lazy<IReadOnlyCollection<Type>> _all_types_defined_by_our_organization = new(() =>
		_all_known_assemblies.Value
			.SelectMany(assembly =>
				assembly
					.GetTypes()
					.SelectMany(t => new [] { t }.Concat(t.GetNestedTypes()))
					.Where(t => t.IsDefinedByOurOrganization()))
			.Distinct()
			.ToArray());

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

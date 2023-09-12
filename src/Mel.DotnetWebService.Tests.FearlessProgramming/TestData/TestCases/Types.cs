using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class Types
{
	public static int Compute()
	{
		return 42;
	}
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
	{
		get
		{
			if (!ConstrainedTypeInfos.IsLoaded(out var loadedAssemblyName) || loadedAssemblyName != Assembly.GetExecutingAssembly().FullName)
			{
				ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly(), forceReload: true);
			}

			var types = TestExecutionEnvironment.All_Known_Assemblies
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

			if (!types.Any(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(ConstrainedFurthermore<>)))
			{
				throw TestDataIntegrityException.GeneratedBy(nameof(Types), nameof(AllConcreteConstrainedTypes), $"expected to generate at least one class with base type {typeof(ConstrainedFurthermore<>).GetName()}, but generated 0.");
			}

			return types;
		}
	}

	public static IReadOnlyCollection<Type> AllConcreteTypes
	{
		get
		{
			var types = TestExecutionEnvironment.All_Known_Assemblies
				.SelectMany(asmb => asmb
					.GetTypes()
					.SelectMany(t => new[] { t }.Concat(t.GetNestedTypes()))
					.Where(t => !t.IsAbstract))
				.Distinct()
				.ToNonEmptyHashSet();

			return types;
		}
	}

	static Type[] _allControllerTypesDefinedByOurOrganization
	=> TestExecutionEnvironment.All_Types_Defined_By_Our_Organization
		.Where(t => t.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.ControllerBase)))
		.ToArray();

	static Type[] _allEnumTypesDefinedByOurOrganization
	=> TestExecutionEnvironment.All_Types_Defined_By_Our_Organization
		.Where(t => t.IsEnum)
		.ToArray();
}

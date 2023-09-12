using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

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
	{
		get
		{
			var types = TestExecutionEnvironment.All_Known_Assemblies
				.SelectMany(asmb => asmb
					.GetTypes()
					.SelectMany(t => new [] { t }.Concat(t.GetNestedTypes()))
					.Where(t => !t.IsAbstract && !t.IsGenericType && ConstrainedTypeInfos.Include(t)))
				.Distinct()
				.ToNonEmptyHashSet();

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

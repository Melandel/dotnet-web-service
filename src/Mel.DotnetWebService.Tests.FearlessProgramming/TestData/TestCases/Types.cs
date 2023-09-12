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
		[_,..] enumTypes => enumTypes,
		_ => throw TestDataIntegrityException.GeneratedBy(typeof(Types), nameof(Types.AllEnumTypesDefinedByOurOrganization), "Found no type matching this criteria")
	};

	static Type[] _allControllerTypesDefinedByOurOrganization
	=> TestExecutionEnvironment.All_Types_Defined_By_Our_Organization
		.Where(t => t.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.ControllerBase)))
		.ToArray();

	static Type[] _allEnumTypesDefinedByOurOrganization
	=> TestExecutionEnvironment.All_Types_Defined_By_Our_Organization
		.Where(t => t.IsEnum)
		.ToArray();
}

using Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class Types
{
	public static IEnumerable<Type> AllEnumTypesDefinedByOurOrganization
	=> TestExecutionEnvironment.All_Known_Assemblies
		.SelectMany(asmb => asmb.GetTypes().Where(t => t.IsDefinedByOurOrganization() && t.IsEnum));
}

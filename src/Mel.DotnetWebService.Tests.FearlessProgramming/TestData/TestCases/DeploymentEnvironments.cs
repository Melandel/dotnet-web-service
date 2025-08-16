using Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

static class DeploymentEnvironments
{
	public static IEnumerable<DeploymentEnvironment> All_DeploymentEnvironments
	=> DeploymentEnvironment.AllPossibleValues;

	public static IEnumerable<DeploymentEnvironment> All_DeploymentEnvironments_Except_Production
	=> All_DeploymentEnvironments.Where(env => env != DeploymentEnvironment.Production);
}

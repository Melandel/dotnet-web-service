namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

static class DeploymentEnvironments
{
	public static IEnumerable<DeploymentEnvironment> All_DeploymentEnvironments
	=> Enum
		.GetValues(typeof(DeploymentEnvironment))
		.Cast<DeploymentEnvironment>();

	public static IEnumerable<DeploymentEnvironment> All_DeploymentEnvironments_Except_Production
	=> All_DeploymentEnvironments.Where(env => env is not DeploymentEnvironment.Production);
}

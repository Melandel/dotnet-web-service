namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

static class HostEnvironments
{
	public static IEnumerable<HostEnvironment> All_Host_Environments
	=> Enum
		.GetValues(typeof(HostEnvironment))
		.Cast<HostEnvironment>();

	public static IEnumerable<HostEnvironment> All_Host_Environments_Except_Production
	=> All_Host_Environments.Where(hostEnv => hostEnv is not HostEnvironment.Production);
}

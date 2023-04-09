namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CodingStandards;

class ControllerTypesMust : TestSuiteUsingTestServer
{
	[TestCaseSource(typeof(Types), nameof(Types.AllControllerTypesDefinedByOurOrganization))]
	public void Inherit_From_AbstractClass_ApiController(Type controllerType)
	{
		Assert.That(
			controllerType.IsEqualOrSubclassOf(typeof(Api.Controllers.ApiController)),
			Is.True);
	}
}

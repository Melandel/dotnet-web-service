namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CodingStandards;

class ControllerTypesMust
{
	static readonly InMemoryTestServer TestServer;
	static ControllerTypesMust()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[TestCaseSource(typeof(Types), nameof(Types.AllControllerTypesDefinedByOurOrganization))]
	public void Inherit_From_AbstractClass_ApiController(Type controllerType)
	{
		Assert.That(
			controllerType.IsEqualOrSubclassOf(typeof(Api.Controllers.ApiController)),
			Is.True);
	}
}

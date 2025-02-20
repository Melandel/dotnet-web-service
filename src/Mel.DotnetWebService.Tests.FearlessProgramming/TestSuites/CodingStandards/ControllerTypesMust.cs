namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CodingStandards;

class ControllerTypesMust
{
	static readonly InMemoryTestServer TestServer;
	static ControllerTypesMust()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[Test]
	public void Inherit_From_AbstractClass_ApiController()
	=> Types.AllControllerTypesDefinedByOurOrganization
		.ParallelForEach(controllerType =>
		{
			Assert.That(
				controllerType.IsEqualOrSubclassOf(typeof(Api.Controllers.ApiController)),
				Is.True);
		});
}

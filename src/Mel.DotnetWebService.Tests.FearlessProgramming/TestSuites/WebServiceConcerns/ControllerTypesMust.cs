namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns;

class ControllerTypesMust : TestSuiteUsingTestServer
{
	[Test]
	public void Inherit_From_AbstractClass_ApiController()
	=> Types.AllControllerTypesDefinedByOurOrganization
		.ParallelForEach(controllerType =>
		{
			Assert.That(
				controllerType.IsEqualOrSubclassOf(typeof(Api.ApiController)),
				Is.True);
		});
}

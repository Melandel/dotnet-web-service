namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.Routing;

class RoutingShould : TestSuiteUsingTestServer
{
	[Test]
	public async Task Create_Urls_Using_KebabCase_AKA_Hyphen_Separated_Lower_Case_Words()
	{
		// Arrange
		var controllerMethod = nameof(ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests.HelloWorld);

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod);
		var response = await httpResponse.GetContentAsString();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(response, Is.EqualTo("hello world"));
	}
}


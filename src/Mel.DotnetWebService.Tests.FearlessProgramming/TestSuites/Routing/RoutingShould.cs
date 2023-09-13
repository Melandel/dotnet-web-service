namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites;

class RoutingShould : TestSuiteUsingTestServer
{
	[Test]
	public async Task Create_Urls_Using_KebabCase_AKA_Hyphen_Separated_Lower_Case_Words()
	{
		// Arrange
		var requestUri = "stubbed-endpoints-specifically-created-for-tests/hello-world";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
		var response = await httpResponse.GetContentAsString();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(response, Is.EqualTo("hello world"));
	}
}


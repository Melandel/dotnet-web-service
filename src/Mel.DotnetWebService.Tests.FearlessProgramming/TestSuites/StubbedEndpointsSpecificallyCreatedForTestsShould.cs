namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites;

class StubbedEndpointsSpecificallyCreatedForTestsShould
{
	[Test]
	public async Task Return_HelloWorld_When_Pinged()
	{
		// Arrange
		var testServer = InMemoryTestServer.Create();
		var requestUri = "StubbedEndpointsSpecificallyCreatedForTests";

		// Act
		var httpResponse = await testServer.HttpClient.GetAsync(requestUri);
		var response = await httpResponse.GetContentAsString();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(response, Is.EqualTo("hello world"));
	}
}

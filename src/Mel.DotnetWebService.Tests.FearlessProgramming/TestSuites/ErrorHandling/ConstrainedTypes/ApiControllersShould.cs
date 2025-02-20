namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling.ConstrainedTypes;

class ApiControllersShould : TestSuiteUsingTestServer
{
	[Test]
	public async Task Receive_Guid_When_Guid_Is_Passed_As_HttpPostRequestBody()
	{
		// Arrange
		var validGuid = "bfa352fd-9e00-49a1-acb2-74f923aa578a";
		var requestUri = $"api/v1/stubbed-endpoints-specifically-created-for-tests/guid-inside-body/";
		var payload = $"\"{validGuid}\"";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync(
			requestUri,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(await httpResponse.ToResponseObject<Guid>(), Is.EqualTo(Guid.Parse(validGuid)));
	}
}

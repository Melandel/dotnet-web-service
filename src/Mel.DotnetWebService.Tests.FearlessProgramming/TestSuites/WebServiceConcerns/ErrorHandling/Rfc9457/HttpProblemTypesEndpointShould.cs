namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.ErrorHandling.Rfc9457;

class HttpProblemTypesEndpointShould : TestSuiteUsingTestServer
{
	static readonly IReadOnlyCollection<string> HttpProblemTypeRoutes;
	static HttpProblemTypesEndpointShould()
	{
		HttpProblemTypeRoutes = TestServer.HttpProblemTypeRoutes;
	}

	[TestCaseSource(nameof(HttpProblemTypeRoutes))]
	public async Task Exist(string httpProblemTypeRoute)
	{
		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(httpProblemTypeRoute);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[TestCaseSource(nameof(HttpProblemTypeRoutes))]
	public async Task Return_NonEmpty_HttpProblemTypes(string httpProblemTypeRoute)
	{
		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(httpProblemTypeRoute);
		var response = await httpResponse.ToResponseObject<HttpProblemTypeArchetype.Deserializable>();

		// Assert
		Assert.That(response, Is.Not.Null);
		Assert.That(response.Uri, Is.Not.Null);
	}

	[TestCaseSource(nameof(HttpProblemTypeRoutes))]
	public async Task Return_Dereferenceable_ProblemTypeUris(string httpProblemTypeRoute)
	{
		// Arrange
		var httpProblemResponse = await TestServer.HttpClient.GetAsync(httpProblemTypeRoute);
		var httpProblem = await httpProblemResponse.ToResponseObject<HttpProblemTypeArchetype.Deserializable>();
		var requestUri = httpProblem.Uri;

		// Act & Assert
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
		var response = await httpResponse.ToResponseObject<HttpProblemTypeArchetype.Deserializable>();

		// Assert
		Assert.That(httpProblem, Is.Not.Null);
		Assert.That(response, Is.Not.Null);
		Assert.That(response.Uri, Is.Not.Null);
	}
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.Routing;

class RoutingShould : TestSuiteUsingTestServer
{
	[TestCaseSource(typeof(WebApiVersionedRoutes), nameof(WebApiVersionedRoutes.ValidRoutesWithGetVerb))]
	public async Task Create_Urls_With_Api_Slash_Version_Prefix_And_KebabCase_AKA_Hyphen_Separated_Lower_Case_Words(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/stubbed-endpoints-specifically-created-for-tests/{actionName}/";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[TestCase("defined-in-all-api-versions")]
	[TestCase("defined-only-in-api-v2")]
	[TestCase("defined-only-in-api-v1-and-api-v2")]
	[TestCase("defined-only-in-api-v1")]
	public async Task Return_404NotFound_For_Undeclared_ApiVersions(string actionName)
	{
		// Arrange
		var undeclaredApiVersion = "v0";
		var requestUri = $"api/{undeclaredApiVersion}/stubbed-endpoints-specifically-created-for-tests/{actionName}/";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
	}

	[TestCase("defined-only-in-api-v2", "v1")]
	[TestCase("defined-only-in-api-v2", "v3")]
	[TestCase("defined-only-in-api-v1-and-api-v2", "v3")]
	[TestCase("defined-only-in-api-v1", "v2")]
	[TestCase("defined-only-in-api-v1", "v3")]
	public async Task Return_404NotFound_For_Undeclared_Actions_In_Declared_ApiVersions(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/stubbed-endpoints-specifically-created-for-tests/{actionName}/";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
	}

	[TestCaseSource(typeof(WebApiVersionedRoutes), nameof(WebApiVersionedRoutes.ValidRoutesWithGetVerb))]
	public async Task Return_405MethodNotAllowed_For_Declared_Actions_In_Declared_ApiVersions_With_Wrong_HttpVerb(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/stubbed-endpoints-specifically-created-for-tests/{actionName}/";

		// Act
		var httpResponse = await TestServer.HttpClient.PatchAsync(requestUri, null);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
	}
}

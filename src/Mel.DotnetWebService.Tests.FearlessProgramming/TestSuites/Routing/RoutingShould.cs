namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.Routing;

public class RoutingShould
{
	static readonly TestServerForRouting TestServer;
	static RoutingShould()
	{
		TestServer = TestServerForRouting.Create();
	}

	[TestCaseSource(typeof(WebApiVersionedRoutes), nameof(WebApiVersionedRoutes.ValidRoutesWithGetVerb))]
	public async Task Create_Endpoints_With_Api_Slash_Version_Prefix(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/WeatherForecast/{actionName}/";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[TestCase("DefinedInAllApiVersions")]
	[TestCase("DefinedOnlyIn_ApiV2")]
	[TestCase("DefinedOnlyIn_ApiV1_ApiV2")]
	[TestCase("DefinedOnlyIn_ApiV1")]
	public async Task Return_404NotFound_For_Undeclared_ApiVersions(string actionName)
	{
		// Arrange
		var undeclaredApiVersion = "v0";
		var requestUri = $"api/{undeclaredApiVersion}/WeatherForecast/{actionName}/";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
	}

	[TestCase("DefinedOnlyIn_ApiV2", "v1")]
	[TestCase("DefinedOnlyIn_ApiV2", "v3")]
	[TestCase("DefinedOnlyIn_ApiV1_ApiV2", "v3")]
	[TestCase("DefinedOnlyIn_ApiV1", "v2")]
	[TestCase("DefinedOnlyIn_ApiV1", "v3")]
	public async Task Return_404NotFound_For_Undeclared_Actions_In_Declared_ApiVersions(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/WeatherForecast/{actionName}/";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
	}

	[TestCaseSource(typeof(WebApiVersionedRoutes), nameof(WebApiVersionedRoutes.ValidRoutesWithGetVerb))]
	public async Task Return_405MethodNotAllowed_For_Declared_Actions_In_Declared_ApiVersions_With_Wrong_HttpVerb(string actionName, string apiVersion)
	{
		// Arrange
		var requestUri = $"api/{apiVersion}/WeatherForecast/{actionName}/";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.PatchAsync(requestUri, null);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
	}
}

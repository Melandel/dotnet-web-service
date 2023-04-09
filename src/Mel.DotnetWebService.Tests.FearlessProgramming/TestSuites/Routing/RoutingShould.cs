using Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.Routing;

public class RoutingShould
{
	[Test]
	public async Task Create_Endpoints_With_Api_Prefix()
	{
		// Arrange
		var testServer = TestServerForRouting.Create();
		var client = testServer.TestFriendlyHttpClient;

		// Act
		var httpResponse = await client.GetAsync("api/WeatherForecast");
		var responseContent = new StreamReader(await httpResponse.Content.ReadAsStreamAsync())
			.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
			Assert.That(responseContent, Is.Not.Null);
		});
	}
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.SwaggerUI;

public class SwaggerUiShould
{
	[Test]
	public async Task Appear_Without_Error_When_Querying_SwaggerUI_Url()
	{
		// Arrange
		var testServer = TestServerForSwaggerUI.Create();
		var client = testServer.HttpClient;
		var swaggerUiUrl = "/swagger/index.html";

		// Act
		var httpResponse = await client.GetAsync(swaggerUiUrl);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
			Assert.That(responseContent, Is.Not.Null);
			Assert.That(responseContent, Contains.Substring("<!DOCTYPE html>"));
			Assert.That(responseContent, Contains.Substring("<title>Swagger UI</title>"));
			Assert.That(responseContent, Does.Not.Contain("Error when attempting to fetch resource."));
		});
	}
};

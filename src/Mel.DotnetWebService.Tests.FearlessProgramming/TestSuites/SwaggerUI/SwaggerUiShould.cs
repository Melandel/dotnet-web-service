﻿namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.SwaggerUI;

class SwaggerUiShould
{
	static readonly InMemoryTestServer TestServer;
	static SwaggerUiShould() => TestServer = InMemoryTestServer.Create();

	[Test]
	public async Task Appear_Without_Error_When_Querying_SwaggerUI_Url()
	{
		// Arrange
		var swaggerUiUrl = "/swagger/index.html";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(swaggerUiUrl);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
			Assert.That(responseContent, Is.Not.Null);
			Assert.That(responseContent, Does.Contain("<!DOCTYPE html>"));
			Assert.That(responseContent, Does.Contain("<title>Swagger UI</title>"));
			Assert.That(responseContent, Does.Not.Contain("Error when attempting to fetch resource."));
		});
	}

	[TestCaseSource(typeof(WebApiRoutes), nameof(WebApiRoutes.CommonlyUsedAsDefaultRoutes))]
	public async Task Appear_Without_Error_When_Querying_Commonly_Used_Default_Routes(string route)
	{
		// Arrange
		var swaggerUiUrl = "/swagger/index.html";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(route);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Redirect));
			Assert.That(httpResponse.Headers.GetValues("Location"), Is.EquivalentTo(new[] { swaggerUiUrl }));
		});
	}

	[TestCaseSource(typeof(WebApiRoutes), nameof(WebApiRoutes.InvalidRoutes))]
	public async Task Not_Appear_When_Querying_Invalid_Routes(string route)
	{
		// Arrange

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(route);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
	}
};

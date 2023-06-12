using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Mel.DotnetWebService.Api.Concerns.ErrorHandling.HttpProblemTypeExtensionMember;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling;

class ErrorHandlingShould
{
	static readonly InMemoryTestServer TestServer;
	static ErrorHandlingShould()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[Test]
	public async Task Implement_Rfc9457()
	{
		// Arrange
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/division-by-zero/";

		// Act & Assert
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			var developerMistake = TestServer.GetHttpProblemTypeByName("developer-mistake");
			Assert.That(problemDetails.Type, Is.EqualTo(developerMistake.Uri.ToString()));
			Assert.That(problemDetails.Title, Is.EqualTo(developerMistake.Title));
			Assert.That(problemDetails.Status, Is.EqualTo((int)developerMistake.RecommendedHttpStatusCode));
			Assert.That(problemDetails.Detail, Is.Not.Null.And.Not.Empty);
		});
	}

	[Test]
	public async Task Not_Leak_Debugging_Information_When_HostEnvironment_Is_Production()
	{
		// Arrange
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/division-by-zero/";
		var testServer = TestServer;

		// Act & Assert
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			Assert.That(problemDetails, Is.Not.Null);
			Assert.That(responseContent, Does.Not.Contain(StackTrace.ToString()).And.Not.Contain(nameof(ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero)));
			Assert.That(responseContent, Does.Not.Contain(ExceptionsTypes.ToString()).And.Not.Contain(nameof(DivideByZeroException)));
			Assert.That(responseContent, Does.Not.Contain(ExceptionsAggregatedMessages.ToString()));
			Assert.That(responseContent, Does.Not.Contain(ExceptionsAggregatedData.ToString()));
		});
	}

	[TestCaseSource(typeof(HostEnvironments), nameof(HostEnvironments.All_Host_Environments_Except_Production))]
	public async Task Return_Debugging_Information_When_HostEnvironment_Is_Not_Production(HostEnvironment hostEnvironment)
	{
		// Arrange
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/division-by-zero/";
		var testServer = InMemoryTestServer.Create(hostEnvironment);

		// Act & Assert
		var httpResponse = await testServer.HttpClient.GetAsync(requestUri);

		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			Assert.That(problemDetails, Is.Not.Null);
			Assert.That(responseContent, Does.Contain(StackTrace.ToString()).And.Contain(nameof(ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero)));
			Assert.That(responseContent, Does.Contain(ExceptionsTypes.ToString()).And.Contain(nameof(DivideByZeroException)));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedMessages.ToString()));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedData.ToString()));
		});
	}
}

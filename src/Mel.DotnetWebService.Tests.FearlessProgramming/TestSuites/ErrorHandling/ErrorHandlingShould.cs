using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling;

public class ErrorHandlingShould
{
	static readonly TestServerForErrorHandling TestServer;
	static ErrorHandlingShould()
	{
		TestServer = TestServerForErrorHandling.Create();
	}

	[Test]
	public async Task Implement_Rfc9457()
	{
		// Arrange
		var requestUri = $"api/v3/WeatherForecast/DivisionByZero/";

		// Act & Assert
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);
		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			Assert.That(problemDetails, Is.Not.Null);
			Assert.That(responseContent, Contains.Substring(DebuggingInformationName.StackTrace.ToString()));

			var developerMistake = TestServer.GetHttpProblemTypeByName("developer-mistake");
			Assert.That(problemDetails.Type, Is.EqualTo(developerMistake.Uri.ToString()));
			Assert.That(problemDetails.Title, Is.EqualTo(developerMistake.Title));
			Assert.That(problemDetails.Status, Is.EqualTo((int)developerMistake.RecommendedHttpStatusCode));
			Assert.That(problemDetails.Detail, Is.Not.Null.And.Not.Empty);
		});
	}
}

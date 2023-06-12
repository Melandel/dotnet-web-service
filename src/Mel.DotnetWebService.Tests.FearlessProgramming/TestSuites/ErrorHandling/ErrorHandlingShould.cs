using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
			Assert.That(responseContent, Contains.Substring(DeveloperMistake.StackTraceDebuggingInformationKey));

			Assert.That(problemDetails.Type, Is.EqualTo(ProblemType.DeveloperMistake.Id.ToString()));
			Assert.That(problemDetails.Title, Is.EqualTo(ProblemType.DeveloperMistake.Name));
			Assert.That(problemDetails.Status, Is.EqualTo((int)ProblemType.DeveloperMistake.HttpStatus));
			Assert.That(problemDetails.Detail, Is.Not.Null.And.Not.Empty);
		});
	}
}

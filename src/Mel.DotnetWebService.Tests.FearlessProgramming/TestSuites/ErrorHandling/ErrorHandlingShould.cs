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
	public async Task Implement_Rfc7807()
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
			Assert.That(problemDetails.Detail, Is.Not.Null.And.Not.Empty);
			Assert.That(problemDetails.Instance, Is.Not.Null.And.Not.Empty);
			Assert.That(problemDetails.Status, Is.Not.Null.And.Not.Zero);
			Assert.That(problemDetails.Title, Is.Not.Null.And.Not.Empty);
			Assert.That(problemDetails.Type, Is.Not.Null.And.Not.Empty);
			Assert.That(problemDetails.Extensions, Is.Not.Null);
		});
	}
}

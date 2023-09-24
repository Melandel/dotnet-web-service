using NUnit.Framework.Interfaces;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns;

class StubbedEndpointsSpecificallyCreatedForTestsShould
{
	static readonly InMemoryTestServer TestServer;
	static StubbedEndpointsSpecificallyCreatedForTestsShould()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[TearDown]
	public async Task LogHttpResponseWhenTestFails()
	{
		if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
		{
			await TestServer.LogHttpResponseMessagesFrom(TestContext.CurrentContext.Test.ID);
		}
	}

	[Test]
	public async Task Return_HelloWorld_When_Pinged()
	{
		// Arrange
		var requestUri = "StubbedEndpointsSpecificallyCreatedForTests";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
		var response = await httpResponse.GetContentAsString();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(response, Is.EqualTo("hello world"));
	}
}

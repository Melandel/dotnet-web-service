using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

internal class InMemoryTestServer : WebApplicationFactory<Program>
{
	readonly Dictionary<string, List<HttpResponseMessage>> _httpResponseMessagesByTestId;
	public TestFriendlyHttpClient HttpClient => CreateHttpClient();
	InMemoryTestServer()
	{
		_httpResponseMessagesByTestId = new Dictionary<string, List<HttpResponseMessage>>();
	}

	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder
			.ConfigureLogging(logging => logging.ClearProviders())
			.ConfigureServices(services => services.AddController<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>());
		return base.CreateHost(builder);
	}

	public static InMemoryTestServer Create()
	=> new InMemoryTestServer();

	public TestFriendlyHttpClient CreateHttpClient()
	{
		var httpClient = CreateDefaultClient();

		var httpResponseMessagesInvolvedInCurrentTest = new List<HttpResponseMessage>();
		_httpResponseMessagesByTestId[TestContext.CurrentContext.Test.ID] = httpResponseMessagesInvolvedInCurrentTest;

		return TestFriendlyHttpClient.ThatStoresHttpResponseMessages(
			httpClient,
			httpResponseMessagesInvolvedInCurrentTest);
	}

	public async Task LogHttpResponseMessagesFrom(string testId)
	{
		if (_httpResponseMessagesByTestId.Remove(testId, out var httpResponseMessagesInvolvedInTheTest))
		{
			foreach (var httpResponseMessage in httpResponseMessagesInvolvedInTheTest)
			{
				TestContext.WriteLine(await httpResponseMessage.Render());
			}
		}
	}
}

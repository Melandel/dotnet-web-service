using Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.ErrorHandling.Rfc9457;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class InMemoryTestServer : WebApplicationFactory<Program>
{
	readonly Dictionary<string, List<HttpResponseMessage>> _httpResponseMessagesByTestId;
	readonly DeploymentEnvironment _deploymentEnvironment;
	readonly Lazy<HttpProblemTypeArchetype.Deserializable[]> _httpProblemTypes;
	public TestFriendlyHttpClient HttpClient => CreateHttpClient();
	InMemoryTestServer(DeploymentEnvironment deploymentEnvironment)
	{
		_httpResponseMessagesByTestId = new Dictionary<string, List<HttpResponseMessage>>();
		_deploymentEnvironment = deploymentEnvironment;
		_httpProblemTypes = new Lazy<HttpProblemTypeArchetype.Deserializable[]>(() =>
		{
			var httpProblemTypesAsHttpResponse = HttpClient.GetAsync("api/v1/http-problem-types")
				.GetAwaiter()
				.GetResult();
			var httpProblemTypes = httpProblemTypesAsHttpResponse.ToResponseObject<HttpProblemTypeArchetype.Deserializable[]>()
				.GetAwaiter()
				.GetResult();
			return httpProblemTypes;
		});
	}
	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder
			.UseEnvironment(_deploymentEnvironment.ToString())
			.ConfigureLogging(logging => logging.ClearProviders())
			.ConfigureServices(services =>
			{
				services
					.AddController<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>();

				services
					.AssertThatServiceIsNotInjected(
						WebServiceShould.Implementent_Rfc9457.TypeOfServiceThatShouldNotBeInjected,
						WebServiceShould.Implementent_Rfc9457.TestThatSpecificallyRequiresServiceNotInjected);
			});

		return base.CreateHost(builder);
	}

	public static InMemoryTestServer Create(DeploymentEnvironment deploymentEnvironment = DeploymentEnvironment.Production)
	=> new InMemoryTestServer(deploymentEnvironment);

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

	public IReadOnlyCollection<string> HttpProblemTypeRoutes
	=> _httpProblemTypes.Value
		.Select(httpProblemType => httpProblemType.Uri.ToString())
		.ToArray();

	public HttpProblemType GetHttpProblemTypeByName(string httpProblemTypeName)
	=> _httpProblemTypes.Value
		.Single(httpProblemType => httpProblemType.Uri.ToString().EndsWith(httpProblemTypeName));
}

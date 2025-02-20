using Mel.DotnetWebService.Api.Concerns.ErrorHandling;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class InMemoryTestServer : WebApplicationFactory<Program>
{
	readonly Dictionary<string, List<HttpResponseMessage>> _httpResponseMessagesByTestId;
	readonly HostEnvironment _hostEnvironment;
	readonly Lazy<HttpProblemTypeArchetype.Deserializable[]> _httpProblemTypes;
	public TestFriendlyHttpClient HttpClient => CreateHttpClient();
	InMemoryTestServer(HostEnvironment hostEnvironment)
	{
		_httpResponseMessagesByTestId = new Dictionary<string, List<HttpResponseMessage>>();
		_hostEnvironment = hostEnvironment;
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
			.UseEnvironment(_hostEnvironment.ToString())
			.ConfigureLogging(logging => logging.ClearProviders())
			.ConfigureServices(services =>
			{
				services.AddController<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>();

				if (services.Any(serviceDescriptor => serviceDescriptor.ImplementationType == typeof(SomeServiceTestDouble.Dummy_That_Is_NOT_Added_To_DependencyInjection)))
				{
					throw TestDataIntegrityException.GeneratedBy(
						typeof(InMemoryTestServer),
						nameof(CreateHost),
						$"the class \"{nameof(SomeServiceTestDouble)}.{nameof(SomeServiceTestDouble.Dummy_That_Is_NOT_Added_To_DependencyInjection)}\" must not be injected for in order for test \"{nameof(ErrorHandlingShould)}.{nameof(ErrorHandlingShould.Implementent_Rfc9457)}.{nameof(ErrorHandlingShould.Implementent_Rfc9457.On_Invalid_DependencyInjectionConfiguration)}()\" to work");
				}
			});

		return base.CreateHost(builder);
	}

	public static InMemoryTestServer Create(HostEnvironment hostEnvironment = HostEnvironment.Production)
	=> new InMemoryTestServer(hostEnvironment);

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

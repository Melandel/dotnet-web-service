using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.ErrorHandling.Rfc9457;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class InMemoryTestServer : WebApplicationFactory<Program>
{
	readonly Dictionary<string, List<HttpResponseMessage>> _httpResponseMessagesByTestId;
	readonly DeploymentEnvironment _deploymentEnvironment;
	readonly Lazy<HttpProblemTypeArchetype.Deserializable[]> _httpProblemTypes;
	public TestFriendlyHttpClient HttpClient => CreateHttpClient();
	public Microsoft.OpenApi.Models.OpenApiDocument OpenApiDocument { get; private set; }
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

		var host = base.CreateHost(builder);
		var openApiDocumentBuilder = host.Services.GetRequiredService<ISwaggerProvider>();
		OpenApiDocument = openApiDocumentBuilder.GetSwagger("v1");

		return host;
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

	public OpenApiOperation GetRouteDocumentation<TController>(string controllerMethod, HttpMethod httpMethod)
	=> GetRouteDocumentationFromDocumentPath<TController>(
		$"/{TestFriendlyHttpClient.BuildRequestUri<TController>(controllerMethod)}",
		httpMethod);

	public OpenApiOperation GetRouteDocumentation<TController>(string controllerMethod, string routeSuffix, HttpMethod httpMethod)
	=> GetRouteDocumentationFromDocumentPath<TController>(
		$"/{TestFriendlyHttpClient.BuildRequestUri<TController>(controllerMethod)}{routeSuffix}",
		httpMethod);

	OpenApiOperation GetRouteDocumentationFromDocumentPath<TController>(string openApiDocumentPath, HttpMethod httpMethod)
	{
		var operationType = httpMethod switch
		{
			_ when httpMethod == HttpMethod.Delete => OperationType.Delete,
			_ when httpMethod == HttpMethod.Get => OperationType.Get,
			_ when httpMethod == HttpMethod.Patch => OperationType.Patch,
			_ when httpMethod == HttpMethod.Post => OperationType.Post,
			_ when httpMethod == HttpMethod.Put => OperationType.Put,
			_ => throw new NotImplementedException()
		};

		return OpenApiDocument.Paths[openApiDocumentPath].Operations[operationType];
	}
}

using Mel.DotnetWebService.Api.Concerns.ErrorHandling;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class InMemoryTestServer : WebApplicationFactory<Program>
{
	public HttpClient HttpClient => CreateDefaultClient(DelegatingHandlerTestDouble.That_Writes_HttpResponse_In_TestOutput);
	readonly HostEnvironment _hostEnvironment;
	readonly Lazy<HttpProblemTypeArchetype.Deserializable[]> _httpProblemTypes;
	InMemoryTestServer(HostEnvironment hostEnvironment)
	{
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
			.ConfigureServices(services => services.AddController<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>());

		return base.CreateHost(builder);
	}

	public IReadOnlyCollection<string> HttpProblemTypeRoutes
	=> _httpProblemTypes.Value
		.Select(httpProblemType => httpProblemType.Uri.ToString())
		.ToArray();

	public HttpProblemType GetHttpProblemTypeByName(string httpProblemTypeName)
	=> _httpProblemTypes.Value
		.Single(httpProblemType => httpProblemType.Uri.ToString().EndsWith(httpProblemTypeName));

	public static InMemoryTestServer Create(HostEnvironment hostEnvironment = HostEnvironment.Production)
	=> new InMemoryTestServer(hostEnvironment);
}

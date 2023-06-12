using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class TestServerForErrorHandling : WebApplicationFactory<Program>
{
	public HttpClient TestFriendlyHttpClient => CreateDefaultClient();
	readonly Lazy<HttpProblemType[]> _httpProblemTypes;
	TestServerForErrorHandling()
	{
		_httpProblemTypes = new Lazy<HttpProblemType[]>(() =>
		{
			var httpProblemTypesAsHttpResponse = TestFriendlyHttpClient.GetAsync("api/v3/HttpProblemTypeProvider").Result;
			var httpProblemTypes = httpProblemTypesAsHttpResponse.ToResponseObject<HttpProblemType[]>().Result;
			return httpProblemTypes;
		});
	}
	protected override IHost CreateHost(IHostBuilder builder)
	{
		return base.CreateHost(builder);
	}

	public HttpProblemType GetHttpProblemTypeByName(string httpProblemTypeName)
	=> _httpProblemTypes.Value
		.Single(httpProblemType => httpProblemType.Uri.ToString().EndsWith(httpProblemTypeName));

	public static TestServerForErrorHandling Create()
	=> new TestServerForErrorHandling();
}


using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class TestServerForEnumTypes : WebApplicationFactory<Program>
{
	public HttpClient TestFriendlyHttpClient => CreateDefaultClient();
	protected override IHost CreateHost(IHostBuilder builder)
	{
		return base.CreateHost(builder);
	}

	public static TestServerForEnumTypes Create()
	=> new TestServerForEnumTypes();
}


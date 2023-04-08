using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

class TestServerForSwaggerUI : WebApplicationFactory<Program>
{
	public HttpClient HttpClient => CreateDefaultClient();
	protected override IHost CreateHost(IHostBuilder builder)
	{
		return base.CreateHost(builder);
	}

	public static TestServerForSwaggerUI Create()
	=> new TestServerForSwaggerUI();
}


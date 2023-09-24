using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestServers;

internal class InMemoryTestServer : WebApplicationFactory<Program>
{
	public HttpClient HttpClient => CreateDefaultClient(DelegatingHandlerTestDouble.That_Writes_HttpResponse_In_TestOutput);
	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder.ConfigureServices(services => services.AddController<ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests>());
		return base.CreateHost(builder);
	}

	public static InMemoryTestServer Create()
	=> new InMemoryTestServer();
}

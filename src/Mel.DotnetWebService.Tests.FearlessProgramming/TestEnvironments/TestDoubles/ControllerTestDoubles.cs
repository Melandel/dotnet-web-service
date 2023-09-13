using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

static class ControllerTestDoubles
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class StubbedEndpointsSpecificallyCreatedForTests : ControllerBase
	{
		readonly ILogger<StubbedEndpointsSpecificallyCreatedForTests> _logger;

		public StubbedEndpointsSpecificallyCreatedForTests(ILogger<StubbedEndpointsSpecificallyCreatedForTests> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public string HelloWorld() => "hello world";
	}
}

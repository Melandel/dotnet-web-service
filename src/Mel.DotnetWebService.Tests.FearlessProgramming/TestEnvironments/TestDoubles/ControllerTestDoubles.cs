using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

static class ControllerTestDoubles
{
	[ApiController]
	[Route("[controller]")]
	public class StubbedEndpointsSpecificallyCreatedForTests : ControllerBase
	{
		private readonly ILogger<StubbedEndpointsSpecificallyCreatedForTests> _logger;

		public StubbedEndpointsSpecificallyCreatedForTests(ILogger<StubbedEndpointsSpecificallyCreatedForTests> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "ping")]
		public string HelloWorld() => "hello world";
	}
}

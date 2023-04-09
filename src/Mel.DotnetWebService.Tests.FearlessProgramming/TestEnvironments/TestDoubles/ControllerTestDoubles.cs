using Mel.DotnetWebService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

static class ControllerTestDoubles
{
	public class StubbedEndpointsSpecificallyCreatedForTests : ApiController
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

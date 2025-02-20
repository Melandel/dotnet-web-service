using Mel.DotnetWebService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

static class ControllerTestDoubles
{

	[ApiVersion("3")]
	[ApiVersion("2", Deprecated = true)]
	[ApiVersion("1", Deprecated = true)]
	public class StubbedEndpointsSpecificallyCreatedForTests : ApiController
	{
		readonly ILogger<StubbedEndpointsSpecificallyCreatedForTests> _logger;
		public StubbedEndpointsSpecificallyCreatedForTests(ILogger<StubbedEndpointsSpecificallyCreatedForTests> logger)
		{
			_logger = logger;
			}

		[MapToApiVersion("3")]
		[HttpGet]
		public JsonResult GetHttpProblemTypeFromAnotherController() => new JsonResult(HttpProblemTypeProvider.GetDeveloperMistake());

		[HttpGet]
		public IActionResult CallSomeService([FromServices] ISomeService someService)
		{
			var input = new ISomeService.Input();
			var output = someService.Process(input);
			return Ok(output);
		}

		[HttpGet]
		public int DivisionByZero()
		{
			int zero = 0;
			return 1/zero;
		}

		[HttpGet]
		public string DefinedInAllApiVersions() => "Hello, word!";

		[MapToApiVersion("2")]
		[HttpGet]
		public string DefinedOnlyInApiV2() => "Hello world!";

		[MapToApiVersion("2")]
		[MapToApiVersion("1")]
		[HttpGet]
		public string DefinedOnlyInApiV1AndApiV2() => "Hello world";

		[MapToApiVersion("1")]
		[HttpGet]
		public string DefinedOnlyInApiV1() => "hello world";

		public record RequestContainingGuidProperty(Guid Guid);

		[HttpPost]
		public Guid GuidPassThrough(RequestContainingGuidProperty request) => request.Guid;

		[HttpGet]
		public Guid GuidPassThrough(Guid guid) => guid;
	}
}

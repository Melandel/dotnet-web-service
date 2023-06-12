using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiVersion("3")]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
public class WeatherForecastController : ApiController
{
	static readonly string[] Summaries = new[]
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	readonly ILogger<WeatherForecastController> _logger;

	public WeatherForecastController(ILogger<WeatherForecastController> logger)
	{
		_logger = logger;
	}

	[MapToApiVersion("3")]
	[HttpGet, Route("GetHttpProblemTypeFromAnotherController")]
	public JsonResult CallsHttpProblemTypesController() => new JsonResult(HttpProblemTypeProvider.GetDeveloperMistake());

	[MapToApiVersion("3")]
	[HttpGet, Route("DivisionByZero")]
	public double DivisionByZero()
	{
		var zero = 0;
		return 1.0/zero;
	}

	[HttpGet, Route("DefinedInAllApiVersions")]
	public string DefinedIn_ApiV1_and_ApiV2_and_ApiV3() => "I am defined in all API versions";

	[MapToApiVersion("2")]
	[HttpGet, Route("DefinedOnlyIn_ApiV2")]
	public string DefinedIn_ApiV2_only() => "I am defined in API version 2 only";

	[MapToApiVersion("2")]
	[MapToApiVersion("1")]
	[HttpGet, Route("DefinedOnlyIn_ApiV1_ApiV2")]
	public string DefinedIn_ApiV1_and_ApiV2() => "I am defined in API versions 1 and 2";

	[MapToApiVersion("1")]
	[HttpGet, Route("DefinedOnlyIn_ApiV1")]
	public IEnumerable<WeatherForecast> DefinedIn_ApiV1()
	{
		return Enumerable.Range(1, 5).Select(index => new WeatherForecast
		{
			Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			TemperatureC = Random.Shared.Next(-20, 55),
			Summary = Summaries[Random.Shared.Next(Summaries.Length)]
		})
		.ToArray();
	}
}

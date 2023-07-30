using Mel.DotnetWebService.Api.EnumsHandling;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiController]
[ProhibitEnumsPassedAsIntegersInRouteOrQueryString]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
	protected HttpProblemTypesController HttpProblemTypeProvider => new HttpProblemTypesController(this);
}

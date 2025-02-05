using Mel.DotnetWebService.Api.Concerns.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api;

[ApiController]
[Concerns.ErrorHandling.ActionFilters.StoreReceivingControllerContextInsideHttpContext]
[Concerns.EnumsHandling.ActionFilters.ProhibitEnumsPassedAsIntegersInRouteOrQueryString]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
	protected HttpProblemTypeProvider HttpProblemTypeProvider => HttpContext.RequestServices.GetRequiredService<HttpProblemTypeProvider>();
}

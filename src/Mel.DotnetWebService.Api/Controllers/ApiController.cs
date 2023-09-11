using Mel.DotnetWebService.Api.EnumsHandling;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiController]
[ErrorHandling.StoreReceivingControllerContextInsideHttpContext]
[ProhibitEnumsPassedAsIntegersInRouteOrQueryString]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
	protected HttpProblemTypeProvider HttpProblemTypeProvider => HttpContext.RequestServices.GetRequiredService<HttpProblemTypeProvider>();
}

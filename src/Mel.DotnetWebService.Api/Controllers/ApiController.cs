using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiController]
[Concerns.ErrorHandling.AccessToReceivingControllerContext.StoreReceivingControllerContextInsideHttpContext]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
	protected HttpProblemTypeProvider HttpProblemTypeProvider => HttpContext.RequestServices.GetRequiredService<HttpProblemTypeProvider>();
}

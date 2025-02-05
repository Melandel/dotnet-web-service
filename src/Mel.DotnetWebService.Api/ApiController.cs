using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api;

[ApiController]
[Concerns.ErrorHandling.Rfc9457.AccessToReceivingControllerContext.StoreReceivingControllerContextInsideHttpContext]
[Concerns.EnumsHandling.IntegerToEnumTypeProhibition.ModelBinding.ProhibitIntegerToEnumTypeBinding]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
	protected HttpProblemTypeProvider HttpProblemTypeProvider => HttpContext.RequestServices.GetRequiredService<HttpProblemTypeProvider>();
}

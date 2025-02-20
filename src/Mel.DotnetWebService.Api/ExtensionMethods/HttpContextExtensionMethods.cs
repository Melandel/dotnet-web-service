using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class HttpContextExtensionMethods
{
	static public void StoreReceivingControllerContext(this HttpContext httpContext, ControllerContext controllerContext)
	=> Concerns.ErrorHandling.Integration.AccessToReceivingControllerContext.StoreInside(controllerContext, httpContext);

	static public ControllerContext? RetrieveReceivingControllerContext(this HttpContext httpContext)
	=> Concerns.ErrorHandling.Integration.AccessToReceivingControllerContext.RetrieveFrom(httpContext);

	static public async Task WriteErrorResponse(this HttpContext httpContext, HttpProblem problem)
	=> await Concerns.ErrorHandling.Integration.ErrorResponseRedaction.Write(problem, httpContext);
}

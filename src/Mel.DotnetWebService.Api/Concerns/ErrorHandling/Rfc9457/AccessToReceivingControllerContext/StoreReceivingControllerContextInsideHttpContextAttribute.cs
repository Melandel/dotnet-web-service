using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.AccessToReceivingControllerContext;

class StoreReceivingControllerContextInsideHttpContextAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var controller = (ControllerBase) context.Controller;
		context.HttpContext.StoreReceivingControllerContext(controller.ControllerContext);
	}
}

using Mel.DotnetWebService.Api.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mel.DotnetWebService.Api.ErrorHandling;

public class StoreReceivingControllerContextInsideHttpContextAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var controller = (ControllerBase) context.Controller;
		context.HttpContext.StoreReceivingControllerContext(controller.ControllerContext);
	}
}

using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

static partial class Integration
{
	public static class AccessToReceivingControllerContext
	{
		const string Key = nameof(AccessToReceivingControllerContext);
		static public void StoreInside(ControllerContext controllerContext, HttpContext httpContext)
		=> httpContext.Items.Add(Key, controllerContext);

		static public ControllerContext? RetrieveFrom(HttpContext httpContext)
		=> httpContext.Items.TryGetValue(Key, out var receivingControllerContext) ? receivingControllerContext as ControllerContext : null;
	}
}

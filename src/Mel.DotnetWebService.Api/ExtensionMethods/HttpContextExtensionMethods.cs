using System.Net;
using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class HttpContextExtensionMethods
{
	const string ReceivingControllerContext = nameof(ReceivingControllerContext);
	static public void StoreReceivingControllerContext(this HttpContext httpContext, ControllerContext controllerContext)
	=> httpContext.Items.Add(ReceivingControllerContext, controllerContext);
	static public ControllerContext? RetrieveReceivingControllerContext(this HttpContext httpContext)
	=> httpContext.Items.TryGetValue(ReceivingControllerContext, out var originalContext) ? originalContext as ControllerContext : null;

	static internal async Task WriteErrorResponse(this HttpContext httpContext, HttpProblem problem)
	{
		httpContext.Response.ContentType = "application/problem+json";
		httpContext.Response.StatusCode = (int) (problem.Type.RecommendedHttpStatusCode ?? HttpStatusCode.InternalServerError);

		var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
		var problemDetailsContext = new ProblemDetailsContext
		{
			HttpContext = httpContext,
			ProblemDetails = problem.ToProblemDetails()
		};

		await problemDetailsService.WriteAsync(problemDetailsContext);
	}
}

using Mel.DotnetWebService.Api.ErrorHandling;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class HttpContextExtensionMethods
{
	static internal async Task WriteErrorResponse(this HttpContext httpContext, Problem problem)
	{
		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = (int) problem.Status;

		var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
		var problemDetailsContext = new ProblemDetailsContext
		{
			HttpContext = httpContext,
			ProblemDetails = problem.ToProblemDetails()
		};

		await problemDetailsService.WriteAsync(problemDetailsContext);
	}
}

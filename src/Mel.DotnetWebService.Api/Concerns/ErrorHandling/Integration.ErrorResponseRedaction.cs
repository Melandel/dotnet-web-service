using System.Net;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

static partial class Integration
{
	public static class ErrorResponseRedaction
	{
		public static async Task Write(HttpProblem problem, HttpContext httpContext)
		{
			httpContext.Response.ContentType = "application/problem+json";
			httpContext.Response.StatusCode = (int)(problem.Type.RecommendedHttpStatusCode ?? HttpStatusCode.InternalServerError);

			var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
			var problemDetailsContext = new ProblemDetailsContext
			{
				HttpContext = httpContext,
				ProblemDetails = problem.ToProblemDetails()
			};

			await problemDetailsService.WriteAsync(problemDetailsContext);
		}
	}
}

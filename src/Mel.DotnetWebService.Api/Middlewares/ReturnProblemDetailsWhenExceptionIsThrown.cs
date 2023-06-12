using Mel.DotnetWebService.Api.ErrorHandling;

namespace Mel.DotnetWebService.Api.Middlewares;

public class ReturnProblemDetailsWhenExceptionIsThrown
{
	readonly RequestDelegate _next;
	public ReturnProblemDetailsWhenExceptionIsThrown(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext httpContext)
	{
		try
		{
			await _next(httpContext);
		}
		catch (Exception ex)
		{
			var exceptionHandlingOutput = ExceptionHandlingOutput.From(httpContext, ex);

			httpContext.Response.StatusCode = exceptionHandlingOutput.StatusCode;
			httpContext.Response.ContentType = exceptionHandlingOutput.ContentType;

			await httpContext.RequestServices.GetRequiredService<IProblemDetailsService>().WriteAsync(
				new ProblemDetailsContext
				{
					HttpContext = httpContext,
					ProblemDetails = exceptionHandlingOutput.ProblemDetails
				}
			);
		}
	}
}

using Mel.DotnetWebService.Api.ExtensionMethods;

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
			var problem = ex.ToProblem();
			await httpContext.WriteErrorResponse(problem);
		}
	}
}

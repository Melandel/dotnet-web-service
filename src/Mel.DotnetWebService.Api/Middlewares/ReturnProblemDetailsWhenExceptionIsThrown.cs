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
			var httpProblemTypeProvider = httpContext.RequestServices.GetRequiredService<Controllers.HttpProblemTypeProvider>();
			var problem = ex.ToProblem(httpProblemTypeProvider);
			await httpContext.WriteErrorResponse(problem);
		}
	}
}

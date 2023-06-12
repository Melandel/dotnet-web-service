namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Middlewares;

class ReturnProblemDetailsWhenExceptionIsThrown
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
			var problem = ex.ToProblem(httpContext);
			await httpContext.WriteErrorResponse(problem);
		}
	}
}

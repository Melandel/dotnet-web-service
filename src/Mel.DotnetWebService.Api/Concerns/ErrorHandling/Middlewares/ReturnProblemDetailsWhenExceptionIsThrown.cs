﻿namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Middlewares;

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
			if (ex.TryConversionToHttpProblem(httpContext, out var httpProblem))
			{
				await httpContext.WriteErrorResponse(httpProblem);
			}
			else
			{
				throw;
			}
		}
	}
}

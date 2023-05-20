using System.Net;

namespace Mel.DotnetWebService.Api.Concerns.Routing.ErrorHandling;

class ReturnEmpty404OrEmpty405DependingOnEndpointResolutionOutcome
{
	readonly RequestDelegate _next;
	public ReturnEmpty404OrEmpty405DependingOnEndpointResolutionOutcome(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext httpContext)
	{
		var endpoint = httpContext.GetEndpoint();
		var httpStatus = DecideHttpStatusFromEndpointResolution(endpoint);

		if (httpStatus is HttpStatusCode.OK)
		{
			await _next(httpContext);
		}
		else
		{
			httpContext.Response.StatusCode = (int)httpStatus;
			return;
		}
	}

	static HttpStatusCode DecideHttpStatusFromEndpointResolution(Endpoint? endpoint)
	=> (endpoint, endpoint?.DisplayName) switch
	{
		(null, _)                                            => HttpStatusCode.NotFound,
		(not RouteEndpoint, "405 HTTP Method Not Supported") => HttpStatusCode.MethodNotAllowed,
		(not RouteEndpoint, _)                               => HttpStatusCode.NotFound,
		(RouteEndpoint, "405 HTTP Method Not Supported")     => HttpStatusCode.MethodNotAllowed,
		_ => HttpStatusCode.OK
	};
}

using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mel.DotnetWebService.Api.EnumsHandling;

public class ProhibitEnumsPassedAsIntegersInRouteOrQueryStringAttribute : ActionFilterAttribute
{
	public override void OnResultExecuting(ResultExecutingContext context)
	{
		var enumParameters = context.ActionDescriptor.Parameters
			.Where(descriptor => descriptor.ParameterType.IsEnum)
			.Select(descriptor => new { descriptor.ParameterType, descriptor.Name });

		var parametersFromQueryString = context.HttpContext.Request.Query;
		var parametersFromRoute = context.HttpContext.Request.RouteValues;
		foreach (var param in enumParameters)
		{
			if (parametersFromQueryString.TryGetValue(param.Name, out var stringValues) && int.TryParse(stringValues.FirstOrDefault(), out var i))
			{
				throw EnumValueReceivedFromIntegerException.From(i, param.Name, Enum.GetNames(param.ParameterType));
			}

			if (parametersFromRoute.TryGetValue(param.Name, out var value) && int.TryParse((string) (value ?? ""), out var j))
			{
				throw EnumValueReceivedFromIntegerException.From(j, param.Name, Enum.GetNames(param.ParameterType));
			}
		}

		base.OnResultExecuting(context);
	}
}

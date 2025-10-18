﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.ActionFilters;

class ProhibitEnumsPassedAsIntegersInRouteOrQueryStringAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var enumParameters = context.ActionDescriptor.Parameters
			.Where(descriptor => descriptor.ParameterType.IsEnum)
			.Select(descriptor => new { descriptor.ParameterType, descriptor.Name });

		var parametersFromQueryString = context.HttpContext.Request.Query;
		var parametersFromRoute = context.HttpContext.Request.RouteValues;
		foreach (var param in enumParameters)
		{
			if (parametersFromQueryString.TryGetValue(param.Name, out var stringValues) && int.TryParse(stringValues.FirstOrDefault(), out var enumValueFromQueryString))
			{
				throw EnumValueReceivedFromIntegerException.From(
					enumValueFromQueryString,
					param.Name,
					Enum.GetNames(param.ParameterType).Where(name => name is not "TechnicalDefaultEnumValue"));
			}

			if (parametersFromRoute.TryGetValue(param.Name, out var value) && int.TryParse((string) (value ?? ""), out var enumValueFromRoute))
			{
				throw EnumValueReceivedFromIntegerException.From(
					enumValueFromRoute,
					param.Name,
					Enum.GetNames(param.ParameterType).Where(name => name is not "TechnicalDefaultEnumValue"));
			}
		}

		base.OnActionExecuting(context);
	}
}

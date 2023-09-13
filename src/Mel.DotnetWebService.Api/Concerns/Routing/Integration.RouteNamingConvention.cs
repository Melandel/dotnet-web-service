using Mel.DotnetWebService.Api.Concerns.Routing.RouteNamingConvention;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mel.DotnetWebService.Api.Concerns.Routing;

partial class Integration
{
	public static class RouteNamingConvention
	{
		public static readonly RouteTokenTransformerConvention KebabCaseTransformerConvention = new RouteTokenTransformerConvention(new KebabCaseParameterTransformer());
	}
}

using Microsoft.AspNetCore.Rewrite;

namespace Mel.DotnetWebService.Api.Concerns.SwaggerUI;

static partial class Integration
{
	public static class Routing
	{
		public static readonly string[] RoutePatternsThatAreCommonlyUsedAsDefaultApiRoute = new[] {
			"^$",
			"^home.html$",
			"^index.html$"
		};
		public static RewriteOptions RewriteCommonlyUsedDefaultApiRoutePatternsIntoSwaggerUiRoute
		{
			get
			{
				var rewriteOptions = new RewriteOptions();
				foreach (var routePattern in RoutePatternsThatAreCommonlyUsedAsDefaultApiRoute)
				{
					rewriteOptions.AddRedirect(routePattern, AccessToSwaggerUI.Route);
				}

				return rewriteOptions;
			}
		}
	}
}

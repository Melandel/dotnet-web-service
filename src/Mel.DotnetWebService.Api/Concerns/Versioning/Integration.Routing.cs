namespace Mel.DotnetWebService.Api.Concerns.Versioning;

static partial class Integration
{
	public static class Routing
	{
		public const string ApiVersionFormatInsideUri = "'v'V"; // 👈 https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings
	}
}

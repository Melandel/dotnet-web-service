using System.Text.RegularExpressions;

namespace Mel.DotnetWebService.Api.Concerns.Routing.RouteNamingConvention;

class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
	readonly KebabCaseConverter _casingConverter = new KebabCaseConverter();

	public string? TransformOutbound(object? value) => _casingConverter.Convert(RemoveAnyHttpVerbPrefix(value));

	public static string? RemoveAnyHttpVerbPrefix(object? value)
	=> value?.ToString() switch
	{
		null => null,
		var str when PrefixedByGetHttpVerb.IsMatch(str)     => str.Substring(HttpMethods.Get.Length),
		var str when PrefixedByHeadHttpVerb.IsMatch(str)    => str.Substring(HttpMethods.Head.Length),
		var str when PrefixedByPostHttpVerb.IsMatch(str)    => str.Substring(HttpMethods.Post.Length),
		var str when PrefixedByPutHttpVerb.IsMatch(str)     => str.Substring(HttpMethods.Put.Length),
		var str when PrefixedByDeleteHttpVerb.IsMatch(str)  => str.Substring(HttpMethods.Delete.Length),
		var str when PrefixedByConnectHttpVerb.IsMatch(str) => str.Substring(HttpMethods.Connect.Length),
		var str when PrefixedByOptionsHttpVerb.IsMatch(str) => str.Substring(HttpMethods.Options.Length),
		var str when PrefixedByTraceHttpVerb.IsMatch(str)   => str.Substring(HttpMethods.Trace.Length),
		var str when PrefixedByPatchHttpVerb.IsMatch(str)   => str.Substring(HttpMethods.Patch.Length),
		var str  => str
	};
	static readonly Regex PrefixedByGetHttpVerb     = new Regex($"^(?i){HttpMethods.Get}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByHeadHttpVerb    = new Regex($"^(?i){HttpMethods.Head}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByPostHttpVerb    = new Regex($"^(?i){HttpMethods.Post}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByPutHttpVerb     = new Regex($"^(?i){HttpMethods.Put}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByDeleteHttpVerb  = new Regex($"^(?i){HttpMethods.Delete}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByConnectHttpVerb = new Regex($"^(?i){HttpMethods.Connect}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByOptionsHttpVerb = new Regex($"^(?i){HttpMethods.Options}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByTraceHttpVerb   = new Regex($"^(?i){HttpMethods.Trace}(?-i)[A-Z_]", RegexOptions.Compiled);
	static readonly Regex PrefixedByPatchHttpVerb   = new Regex($"^(?i){HttpMethods.Patch}(?-i)[A-Z_]", RegexOptions.Compiled);
}

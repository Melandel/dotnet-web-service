namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.PerformanceBenchmarks;

class PerformanceBenchmarkAttribute : IgnoreAttribute
{
	public PerformanceBenchmarkAttribute()
		: base($"{nameof(PerformanceBenchmarkAttribute).Replace("Attribute", "")}s can be locally run by developers for informative purposes but are ignored by default. If you want to run them, remove the {nameof(PerformanceBenchmarkAttribute)} on the desired test method or class.")
	{
	}
}

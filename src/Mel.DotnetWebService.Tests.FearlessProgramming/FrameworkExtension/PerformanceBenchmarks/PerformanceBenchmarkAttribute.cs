namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.PerformanceBenchmarks;

class PerformanceBenchmarkAttribute : ExplicitAttribute
{
	public PerformanceBenchmarkAttribute()
		: base($"{nameof(PerformanceBenchmarkAttribute).Replace("Attribute", "")}s are run only when explicitly selected (cf https://docs.nunit.org/articles/nunit/writing-tests/attributes/explicit.html#trick)")
	{
	}
}

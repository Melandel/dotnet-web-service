using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

public static class TestCasesAggregation
{
	public static TestCasesAggregation<TTestCase> CreateFromTestCases<TTestCase>(IEnumerable<TTestCase> testCases, [CallerMemberName] string propertyName = "") where TTestCase : class
	=> TestCasesAggregation<TTestCase>.CreateFromTestCases(testCases, propertyName);

	public static TestCasesAggregation<TTestCase> CreateFromTestCasesAggregations<TTestCase>(IEnumerable<TestCasesAggregation<TTestCase>> testCasesAggregations) where TTestCase : class
	=> TestCasesAggregation<TTestCase>.CreateFromTestCasesAggregations(testCasesAggregations);
}

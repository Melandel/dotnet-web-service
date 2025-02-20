using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class TestCasesAggregationExtensionMethods
{
	public static IEnumerable<TestCasesAggregation<TTestCase>> AddVariations<TTestCase>(this IEnumerable<TestCasesAggregation<TTestCase>> collectionOfAggregatedTestCases, params Func<TTestCase, TTestCase>[] variations)
		where TTestCase: class
	{
		var concatenated = collectionOfAggregatedTestCases;
		foreach (var variation in variations)
		{
			concatenated = concatenated.Concat(collectionOfAggregatedTestCases.CreateVariation(variation));
		}

		return concatenated;
	}

	public static IEnumerable<TestCasesAggregation<TOtherTestCase>> CreateVariation<TTestCase, TOtherTestCase>(this IEnumerable<TestCasesAggregation<TTestCase>> collectionOfAggregatedTestCases, Func<TTestCase, TOtherTestCase> testCaseToOtherTestCase)
		where TTestCase: class
		where TOtherTestCase: class
	=> collectionOfAggregatedTestCases.Select(aggregatedTestCases => aggregatedTestCases.Select(testCaseToOtherTestCase));
}

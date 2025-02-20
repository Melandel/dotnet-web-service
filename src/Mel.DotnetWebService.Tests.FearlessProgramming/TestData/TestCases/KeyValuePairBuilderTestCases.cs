using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class KeyValuePairBuilderTestCases
{
	public record TestCase(Type ValueType);

	public static IEnumerable<TestCasesAggregation<TestCase>> AllTestCasesAggregatedIntoOne
	=> ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOne.Select(testCase => testCase.Select(tc => new TestCase(tc.Type)));
}

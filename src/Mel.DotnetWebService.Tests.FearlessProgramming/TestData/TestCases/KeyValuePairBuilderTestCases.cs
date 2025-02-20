using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class KeyValuePairBuilderTestCases
{
	public record TestCase(Type ValueType);

	// 👇 All the test cases are aggregated into a single test case (in the sense of NUnit's list of individually runnable test)
	//   Justification: a high number of (NUnit) test cases exerts stress on VisualStudio, creating a 3-4 minutes freeze between each test suite run
	public static IEnumerable<TestCasesAggregation<TestCase>> AllTestCasesAggregatedIntoOne
	=> ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOne.Select(testCase => testCase.Select(tc => new TestCase(tc.Type)));
}

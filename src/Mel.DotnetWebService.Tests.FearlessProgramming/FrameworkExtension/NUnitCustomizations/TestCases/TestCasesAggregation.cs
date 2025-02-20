using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

public class TestCasesAggregation<TTestCase> where TTestCase : class
{
	public static implicit operator TestCaseReturnedByAProperty<TTestCase>[](TestCasesAggregation<TTestCase> aggregation) => aggregation._aggregatedTestCases.ToArray();
	readonly IEnumerable<TestCaseReturnedByAProperty<TTestCase>> _aggregatedTestCases;
	TestCasesAggregation(IEnumerable<TestCaseReturnedByAProperty<TTestCase>> values)
	{
		_aggregatedTestCases = values;
	}
	public static TestCasesAggregation<TTestCase> CreateFromTestCases(IEnumerable<TTestCase> testCases, [CallerMemberName] string propertyName = "")
	{
		return new(IncorporatePropertyNameTo(testCases, propertyName));
	}

	public static TestCasesAggregation<TTestCase> CreateFromTestAggregations(IEnumerable<TestCasesAggregation<TTestCase>> testCasesAggregations)
	{
		return new(testCasesAggregations.SelectMany(agg => (TestCaseReturnedByAProperty<TTestCase>[])agg));
	}

	public void ForEach(Action<TTestCase, string> unitaryTest, [CallerFilePath] string testFilePath = "", [CallerMemberName] string testMethodName = "")
	{
		var parentDirectoryName = Path.GetDirectoryName(testFilePath);
		var grandParentDirectoryName = Path.GetDirectoryName(parentDirectoryName);
		var testPath = $"{Path.GetFileName(grandParentDirectoryName)}/{Path.GetFileName(parentDirectoryName)}/{Path.GetFileNameWithoutExtension(testFilePath)}.{testMethodName}";
		using (Assert.EnterMultipleScope())
		{
			foreach ((var testCase, var failingTestMessage) in _aggregatedTestCases)
			{
				var failedTestFeedback = $"{testPath}():{Environment.NewLine}{failingTestMessage}";
				try
				{
					unitaryTest.Invoke(testCase, failedTestFeedback);
				}
				catch
				{
					TestContext.WriteLine(failedTestFeedback);
					throw;
				}
			}
		}
	}

	public TestCasesAggregation<TResult> Select<TResult>(Func<TTestCase, TResult> selectClause) where TResult : class
	=> new(_aggregatedTestCases.Select(tc => TestCaseReturnedByAProperty<TResult>.CreateFrom(selectClause.Invoke(tc.TestCase), tc.PropertyName)));

	public TestCasesAggregation<TTestCase> Where<TNewTestCase>(Func<TTestCase, bool> whereClause) where TNewTestCase : class
	=> new(_aggregatedTestCases.Where(tc => whereClause.Invoke(tc.TestCase)));

	static IEnumerable<TestCaseReturnedByAProperty<TTestCase>> IncorporatePropertyNameTo(IEnumerable<TTestCase> testCases, string testCasesAggregationPropertyName)
	{
		foreach (var testCase in testCases)
		{
			yield return new TestCaseReturnedByAProperty<TTestCase>(testCase, testCasesAggregationPropertyName);
		}
	}
}

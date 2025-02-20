using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

public class TestCaseReturnedByAProperty<TTestCase> where TTestCase : class
{
	public override string ToString() => TestCase.ToString();
	public string PropertyName { get; }
	public TTestCase TestCase { get; }
	public TestCaseReturnedByAProperty(TTestCase testCase, string propertyName)
	{
		TestCase = testCase;
		PropertyName = propertyName;
	}
	public static TestCaseReturnedByAProperty<TTestCase> CreateFrom(TTestCase testCase, [CallerMemberName] string propertyName = "")
	=> new(testCase, propertyName);

	public void Deconstruct(out TTestCase testCase, out string failingTestMessage)
	{
		testCase = TestCase;
		failingTestMessage = $"{typeof(TTestCase).GetName()} within {PropertyName} has failed:{Environment.NewLine}{TestCase.GetStringRepresentation(indent: true)}";
	}
}

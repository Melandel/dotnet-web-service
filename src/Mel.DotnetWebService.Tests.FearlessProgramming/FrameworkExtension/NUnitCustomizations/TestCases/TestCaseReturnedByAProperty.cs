using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

public static class TestCaseReturnedByAProperty
{
	public static TestCaseReturnedByAProperty<TTestCase> AddCurrentPropertyNameTo<TTestCase>(TTestCase testCase, [CallerMemberName] string propertyName = "")
		where TTestCase : class
	=> TestCaseReturnedByAProperty<TTestCase>.CreateFrom(testCase, propertyName);
}

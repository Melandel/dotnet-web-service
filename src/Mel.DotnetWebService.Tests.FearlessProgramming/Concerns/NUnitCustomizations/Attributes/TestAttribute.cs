using NUnit.Framework.Interfaces;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.NUnitCustomizations.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestAttribute : NUnit.Framework.TestAttribute, ITestAction
{
	public void BeforeTest(ITest test)
	{
	}

	public void AfterTest(ITest test)
	{
		TestDataGeneration.Randomization.Integration.LogHintOnFailingTest();
	}

	public ActionTargets Targets => ActionTargets.Test;
}


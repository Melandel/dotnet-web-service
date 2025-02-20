using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites;

[SetUpFixture]
public class SetupFixture
{
	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly(), forceReload: true);
	}

	[OneTimeTearDown]
	public void RunAfterAnyTests()
	{
	}
}

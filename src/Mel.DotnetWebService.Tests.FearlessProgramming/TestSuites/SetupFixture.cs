using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites;

[SetUpFixture]
public class SetupFixture
{
	[OneTimeSetUp]
	public void RunBeforeFirstTest()
	{
		ConstrainedTypesLoader.LoadConstrainedTypesFromExecutingAssemblyIfNecessary();
	}
}

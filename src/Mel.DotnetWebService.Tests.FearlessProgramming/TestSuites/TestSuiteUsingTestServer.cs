using NUnit.Framework.Interfaces;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites;

abstract class TestSuiteUsingTestServer
{
	protected static readonly InMemoryTestServer TestServer;
	static TestSuiteUsingTestServer()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[TearDown]
	public async Task LogHttpResponseWhenTestFails()
	{
		if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
		{
			await TestServer.LogHttpResponseMessagesFrom(TestContext.CurrentContext.Test.ID);
		}
	}
}

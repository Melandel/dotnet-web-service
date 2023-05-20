namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class WebApiVersionedRoutes
{
	public static IEnumerable<object> ValidRoutesWithGetVerb
	{
		get
		{
			yield return new object[] { "defined-in-all-api-versions",   "v1" };
			yield return new object[] { "defined-in-all-api-versions",   "v2" };
			yield return new object[] { "defined-in-all-api-versions",   "v3" };
			yield return new object[] { "defined-only-in-api-v2",       "v2" };
			yield return new object[] { "defined-only-in-api-v1-and-api-v2", "v1" };
			yield return new object[] { "defined-only-in-api-v1-and-api-v2", "v2" };
			yield return new object[] { "defined-only-in-api-v1",       "v1" };
		}
	}
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class WebApiVersionedRoutes
{
	public static IEnumerable<object> ValidRoutesWithGetVerb
	{
		get
		{
			yield return new object[] { "DefinedInAllApiVersions",   "v1" };
			yield return new object[] { "DefinedInAllApiVersions",   "v2" };
			yield return new object[] { "DefinedInAllApiVersions",   "v3" };
			yield return new object[] { "DefinedOnlyIn_ApiV2",       "v2" };
			yield return new object[] { "DefinedOnlyIn_ApiV1_ApiV2", "v1" };
			yield return new object[] { "DefinedOnlyIn_ApiV1_ApiV2", "v2" };
			yield return new object[] { "DefinedOnlyIn_ApiV1",       "v1" };
		}
	}
}

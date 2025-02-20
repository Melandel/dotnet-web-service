namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class WebApiRoutes
{
	public static IEnumerable<object> CommonlyUsedAsDefaultRoutes
	{
		get
		{
			yield return new object[] { "" };
			yield return new object[] { "index.html" };
			yield return new object[] { "home.html" };
		}
	}

	public static IEnumerable<object> InvalidRoutes
	{
		get
		{
			var i = 0;
			yield return new object[] { string.Format("{0}",                Some.Valid<NonEmptyString>(i))   };
			yield return new object[] { string.Format("{0}/",               Some.Valid<NonEmptyString>(i++)) };
			yield return new object[] { string.Format("{0}/{1}",            Some.Valid<NonEmptyString>(i++), Some.Valid<NonEmptyString>(i++)) };
			yield return new object[] { string.Format("{0}/{1}/index.html", Some.Valid<NonEmptyString>(i++), Some.Valid<NonEmptyString>(i++)) };
		}
	}
}

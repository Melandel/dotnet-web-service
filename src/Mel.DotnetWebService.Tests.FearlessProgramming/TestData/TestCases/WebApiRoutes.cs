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
			yield return new object[] { StringArchetype.Foo };
			yield return new object[] { $"{StringArchetype.Bar}/" };
			yield return new object[] { $"{StringArchetype.Foobar}/{StringArchetype.Baz}" };
			yield return new object[] { $"{StringArchetype.Qux}/{StringArchetype.Quux}/index.html" };
		}
	}
}

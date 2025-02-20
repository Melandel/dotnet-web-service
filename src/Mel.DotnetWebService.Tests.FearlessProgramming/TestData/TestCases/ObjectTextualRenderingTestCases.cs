using System.Collections;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class ObjectTextualRenderingTestCases : IEnumerable
{
	public IEnumerator GetEnumerator()
	{
		yield return new object[] { null, "null" };

		foreach(var kvp in Cases)
		{
			yield return new object[] { kvp.Key, kvp.Value };
		}
	}

	static readonly Dictionary<object, string> Cases = new Dictionary<object, string>
	{
		{ "a", "\"a\"" },
		{ "é", "\"é\"" },
		{ "A", "\"A\"" },
	};
}

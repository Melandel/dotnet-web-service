using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static class StringArchetype
{
	public static string SomeValue(int salt = 0) => AllValues[salt % AllValues.Length];
	static readonly string[] AllValues = typeof(StringArchetype)
		.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
		.Where(field => field.IsLiteral && !field.IsInitOnly)
		.Select(field => field.GetRawConstantValue() as string)
		.ToArray();

	public const string Foo = "foo";
	public const string Bar = "bar";
	public const string Foobar = "foobar";
	public const string Baz = "baz";
	public const string Qux = "qux";
	public const string Quux = "quux";
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.DotnetPrimitiveTypes;

public static class StringArchetype
{
	public static string SomeValue => Foo;
	public static string SomeOtherValue => Bar;
	public static string YetAnotherValue => Foobar;

	public const string Foo = "foo";
	public const string Bar = "bar";
	public const string Foobar = "foobar";
	public const string Baz = "baz";
	public const string Qux = "qux";
	public const string Quux = "quux";

	public const string SomeGuid       = "00000000-0000-0000-0000-000000000001";
	public const string SomeOtherGuid  = "00000000-0000-0000-0000-000000000002";
	public const string YetAnotherGuid = "00000000-0000-0000-0000-000000000003";

	public static string CurrentNamespacePrefix(int numberOfNamespaceComponents)
	=> string.Join('.', typeof(StringArchetype).Namespace.Split('.').Take(numberOfNamespaceComponents));
}

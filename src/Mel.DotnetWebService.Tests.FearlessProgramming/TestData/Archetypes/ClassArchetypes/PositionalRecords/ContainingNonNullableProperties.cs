namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
#nullable enable annotations
	public record ContainingNonNullableProperties(string String, PositiveInt PositiveInt, int? NullableInt);
}

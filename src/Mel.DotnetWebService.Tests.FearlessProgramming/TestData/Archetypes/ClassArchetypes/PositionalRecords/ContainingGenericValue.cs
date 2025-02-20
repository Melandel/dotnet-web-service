namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record PositionalRecordTypeContainingGenericValue<TValue>(TValue Value);
}

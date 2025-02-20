namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record PositionalRecordTypeContainingConstrainedValuePropertyTypes(int IntProperty, NonEmptyGuid NonEmptyGuidProperty);
}

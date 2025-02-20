namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record PositionalRecordTypeContainingSelfReference(int IntProperty, PositionalRecordTypeContainingSelfReference InnerInstance);
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.DotnetPrimitiveTypes;

public static class GuidArchetype
{
	public static readonly Guid SomeValue = Guid.Parse(StringArchetype.SomeGuid);
	public static readonly Guid SomeOtherValue = Guid.Parse(StringArchetype.SomeOtherGuid);
	public static readonly Guid YetAnotherValue = Guid.Parse(StringArchetype.YetAnotherGuid);
	public static Guid Random => Guid.NewGuid();
}

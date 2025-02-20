namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class PublicSingletonFieldExposingType
	{
		public static readonly PublicSingletonFieldExposingType Instance = new();
		PublicSingletonFieldExposingType() { }
	}
}


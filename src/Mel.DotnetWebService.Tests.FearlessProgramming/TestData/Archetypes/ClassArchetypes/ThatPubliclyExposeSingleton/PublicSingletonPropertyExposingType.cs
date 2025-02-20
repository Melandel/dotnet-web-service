namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class PublicSingletonPropertyExposingType
	{
		static readonly PublicSingletonPropertyExposingType _instance = new();
		public static PublicSingletonPropertyExposingType Instance => _instance;
		PublicSingletonPropertyExposingType() { }
	}
}


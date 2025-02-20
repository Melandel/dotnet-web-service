namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class MultiplePublicConstructorsHavingParametersExposingType
	{
		public string BuiltFrom { get; }
		public MultiplePublicConstructorsHavingParametersExposingType(int i) => BuiltFrom = $"single parameter constructor called with value {i}";
		public MultiplePublicConstructorsHavingParametersExposingType(int i, int j) => BuiltFrom = $"two-parameters constructor called with values ({i},{j})";
	}
}

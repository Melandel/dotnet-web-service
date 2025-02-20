namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class MultiplePublicStaticFactoryMethodsExposingType
	{
		public string BuiltFrom { get; }
		MultiplePublicStaticFactoryMethodsExposingType(string builtFrom) => BuiltFrom = builtFrom;
		public static MultiplePublicStaticFactoryMethodsExposingType CreateFrom(int i)        => new MultiplePublicStaticFactoryMethodsExposingType($"single parameter static factory method called with value {i}");
		public static MultiplePublicStaticFactoryMethodsExposingType CreateFrom(int i, int j) => new MultiplePublicStaticFactoryMethodsExposingType($"two-parameters static factory method called with values ({i},{j})");
	}
}


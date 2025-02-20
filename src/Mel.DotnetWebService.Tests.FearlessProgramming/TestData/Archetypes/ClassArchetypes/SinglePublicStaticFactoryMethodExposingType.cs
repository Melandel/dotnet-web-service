namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class SinglePublicStaticFactoryMethodExposingType
	{
		public int Value { get; }
		SinglePublicStaticFactoryMethodExposingType(int i) => Value = i;
		public static SinglePublicStaticFactoryMethodExposingType CreateFrom(int i) => new SinglePublicStaticFactoryMethodExposingType(i);
	}
}

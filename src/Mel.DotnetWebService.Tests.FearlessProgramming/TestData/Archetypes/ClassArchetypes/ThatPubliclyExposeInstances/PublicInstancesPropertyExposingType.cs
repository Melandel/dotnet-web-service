namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class PublicInstancesPropertyExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesPropertyExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static PublicInstancesPropertyExposingType FirstExposed => new(nameof(FirstExposed), 1);
		public static PublicInstancesPropertyExposingType SecondExposed => new(nameof(SecondExposed), 2);
	}
}


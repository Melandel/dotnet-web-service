namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class PublicInstancesFieldExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesFieldExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static PublicInstancesFieldExposingType FirstExposed = new(nameof(FirstExposed), 1);
		public static PublicInstancesFieldExposingType SecondExposed = new(nameof(SecondExposed), 2);
	}
}


namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public class PublicInstancesReadonlyFieldExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesReadonlyFieldExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static readonly PublicInstancesReadonlyFieldExposingType FirstExposed = new(nameof(FirstExposed), 1);
		public static readonly PublicInstancesReadonlyFieldExposingType SecondExposed = new(nameof(SecondExposed), 2);
	}
}


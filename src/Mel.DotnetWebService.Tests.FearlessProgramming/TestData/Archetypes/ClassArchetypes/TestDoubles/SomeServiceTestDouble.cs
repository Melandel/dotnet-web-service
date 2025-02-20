namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record SomeServiceTestDouble : TestDouble<InterfaceArchetype.ISomeService, SomeServiceTestDouble>, InterfaceArchetype.ISomeService
	{
		public InterfaceArchetype.ISomeService.Output Process(InterfaceArchetype.ISomeService.Input input)
		=> new InterfaceArchetype.ISomeService.Output();
	}
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class InterfaceArchetype
{
	public interface ISomeService
	{
		public record Input();
		public record Output();
		public Output Process(Input input);
	}
}

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

interface ISomeService
{
	public record Input();
	public record Output();
	public Output Process(Input input);
}

static class SomeServiceTestDouble
{
	public class Dummy_That_Is_NOT_Added_To_DependencyInjection : ISomeService
	{
		ISomeService.Output ISomeService.Process(ISomeService.Input input)
		=> new ISomeService.Output();
	}
}

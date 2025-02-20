namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Generator;

public record RandomizedExampleValueGenerationResult<T>(T Value, Queue<int> GenerativeSalts);

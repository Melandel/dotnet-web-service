namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

public record RandomizedExampleValueGenerationResult<T>(T Value, Queue<int> GenerativeSalts);

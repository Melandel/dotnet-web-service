using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders;

static class Randomized
{
	public static RandomizedValid<T> Valid<T>()
	{
		LoadContrainedTypesIfNecessary();
		return RandomizedExampleValueGenerator.GenerateExampleOf<T>().ToRandomizedValid();
	}

	public static RandomizedValid<T> Valid<T, TExampleFrom>()
	{
		LoadContrainedTypesIfNecessary();
		return RandomizedExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>().ToRandomizedValid();
	}

	static void LoadContrainedTypesIfNecessary()
	{
		if (!ConstrainedTypeInfos.IsLoaded(out _))
		{
			ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly());
		}
	}
}

public class RandomizedValid<T>
{
	public T Value { get; }
	public Queue<int> GenerativeSalts { get; }
	public string FailingTestMessage => $"Test failed for value {nameof(Regenerate)}.{nameof(Regenerate.Valid)}<{typeof(T).GetName()}>({GenerativeSalts.GetStringRepresentation()}){Environment.NewLine}{Value.GetStringRepresentation(true)}";
	public RandomizedValid(RandomizedExampleValueGenerationResult<T> exampleGenerationResult)
	{
		Value = exampleGenerationResult.Value;
		GenerativeSalts = exampleGenerationResult.GenerativeSalts;
	}

	public static implicit operator T(RandomizedValid<T> obj) => obj.Value;
}

public static class RandomizedExampleValueGenerationResultMethodExtensions
{
	public static RandomizedValid<T> ToRandomizedValid<T>(this RandomizedExampleValueGenerationResult<T> generationResult)
	=> new RandomizedValid<T>(generationResult);
}

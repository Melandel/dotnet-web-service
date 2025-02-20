using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders;

static class Regenerate
{
	public static T Valid<T>(IEnumerable<int> generativeSalts)
	{
		LoadContrainedTypesIfNecessary();
		return RandomizedExampleRegenerator.RegenerateRandomizedExampleOf<T>(new Queue<int>(generativeSalts));
	}

	public static T Valid<T, TExampleFrom>(IEnumerable<int> generativeSalts)
	{
		LoadContrainedTypesIfNecessary();
		return RandomizedExampleRegenerator.RegenerateRandomizedExampleOf<T, TExampleFrom>(new Queue<int>(generativeSalts));
	}

	static void LoadContrainedTypesIfNecessary()
	{
		if (!ConstrainedTypeInfos.IsLoaded(out _))
		{
			ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly());
		}
	}
}


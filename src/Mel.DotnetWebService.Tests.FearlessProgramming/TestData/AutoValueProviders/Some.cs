using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders;

static class Some
{
	static readonly int SaltLevel = 0;

	public static T Valid<T>(int salt = 0)
	{
		LoadContrainedTypesIfNecessary();
		return ExampleValueGenerator.GenerateExampleOf<T>(SaltLevel + salt);
	}

	public static T Valid<T, TExampleFrom>(int salt = 0)
	{
		LoadContrainedTypesIfNecessary();
		return ExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>(SaltLevel + salt);
	}

	public static object Valid(Type type, int salt = 0)
	{
		LoadContrainedTypesIfNecessary();
		return ExampleValueGenerator.GenerateExampleOf(type, null, SaltLevel + salt);
	}

	public static object Valid(Type type, Type source, int salt = 0)
	{
		LoadContrainedTypesIfNecessary();
		return ExampleValueGenerator.GenerateExampleOf(type, source, SaltLevel + salt);
	}

	static void LoadContrainedTypesIfNecessary()
	{
		if (!ConstrainedTypeInfos.IsLoaded(out _))
		{
			ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly());
		}
	}
}

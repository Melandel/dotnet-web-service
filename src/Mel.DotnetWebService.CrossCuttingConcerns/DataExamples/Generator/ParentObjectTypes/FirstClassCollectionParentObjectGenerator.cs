using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class FirstClassCollectionParentObjectGenerator : ParentObjectGenerator
{
	static readonly FirstClassCollectionParentObjectGenerator _instance = new();
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = [];
	public static FirstClassCollectionParentObjectGenerator Instance(Type targetType, Type? constructorParameterType, Type[] instanciationOperationParameterTypeCandidates)
	{
		if (!InstanciationOperationParameterTypesByTypeToConstruct.ContainsKey(targetType))
		{
			var instanciationOperationParameterType = ResolveBestInstanciationTypeCandidate(instanciationOperationParameterTypeCandidates, constructorParameterType);
			InstanciationOperationParameterTypesByTypeToConstruct.Add(targetType, instanciationOperationParameterType);
		}
		return _instance;
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[type];
		var collection = ExampleValueGenerator.GenerateExampleOf(instanciationParameterType, salt);
		var firstClassCollection = type.CreateInstanceUsingConstructorOrFactoryMethod(collection, BindingFlags.Public);
		return firstClassCollection;
	}

	static Type ResolveBestInstanciationTypeCandidate(Type[] instanciationParameterTypeCandidates, Type? constructorParameter)
	{
		if (instanciationParameterTypeCandidates.Contains(constructorParameter))
		{
			return constructorParameter!;
		}

		var instanciationParametersAssignableFromConstructorParameter = instanciationParameterTypeCandidates
			.Where(instanciationParameter => instanciationParameter.IsAssignableFrom(constructorParameter))
			.ToArray();
		if (instanciationParametersAssignableFromConstructorParameter.Length != 0)
		{
			return instanciationParametersAssignableFromConstructorParameter.First();
		}

		var instanciationParametersAssignableToConstructorParameter = instanciationParameterTypeCandidates
			.Where(instanciationParameter => instanciationParameter.IsAssignableTo(constructorParameter))
			.ToArray();
		if (instanciationParametersAssignableToConstructorParameter.Length != 0)
		{
			return instanciationParametersAssignableToConstructorParameter.First();
		}

		return instanciationParameterTypeCandidates.First();
	}
}

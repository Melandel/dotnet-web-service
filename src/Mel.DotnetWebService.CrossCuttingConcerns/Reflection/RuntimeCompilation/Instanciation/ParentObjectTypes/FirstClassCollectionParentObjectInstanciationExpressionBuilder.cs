using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class FirstClassCollectionParentObjectInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly FirstClassCollectionParentObjectInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = [];
	public static FirstClassCollectionParentObjectInstanciationExpressionBuilder Instance(Type targetType, Type? constructorParameterType, Type[] instanciationOperationParameterTypeCandidates)
	{
		if (!InstanciationOperationParameterTypesByTypeToConstruct.ContainsKey(targetType))
		{
			var instanciationOperationParameterType = ResolveBestInstanciationTypeCandidate(instanciationOperationParameterTypeCandidates, constructorParameterType);
			InstanciationOperationParameterTypesByTypeToConstruct.Add(targetType, instanciationOperationParameterType);
		}
		return _instance;
	}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[type];
		// var collection = InstanciationExpressionBuilder.BuildInstanciationExpressionFor(instanciationParameterType, salt);
		// var firstClassCollection = type.CreateInstanceUsingConstructorOrFactoryMethod(collection, BindingFlags.Public);
		// return firstClassCollection;
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

using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class NonGenericConstrainedCollectionParentObjectGenerator : ParentObjectGenerator
{
	static readonly NonGenericConstrainedCollectionParentObjectGenerator _instance = new();
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = [];
	public static NonGenericConstrainedCollectionParentObjectGenerator Instance(Type targetType, Type instanciationOperationParameterTypeCandidates)
	{
		InstanciationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instanciationOperationParameterTypeCandidates);
		return _instance;
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[type];
		var collection = ExampleValueGenerator.GenerateExampleOf(instanciationParameterType, salt);
		var firstClassCollection = type.CreateInstanceUsingConstructorOrFactoryMethod(collection, BindingFlags.Public);
		return firstClassCollection;
	}
}

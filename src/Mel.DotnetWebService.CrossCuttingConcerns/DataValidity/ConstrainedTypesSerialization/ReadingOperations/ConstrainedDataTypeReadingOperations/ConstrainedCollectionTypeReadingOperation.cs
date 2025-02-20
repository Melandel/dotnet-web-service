using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

class ConstrainedCollectionTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedCollectionTypeReadingOperation _instance = new();
	public static ConstrainedCollectionTypeReadingOperation Instance(Type targetType, Type? constructorParameterType, Type[] instanciationOperationParameterTypeCandidates)
	{
		if (!InstanciationOperationParameterTypesByTypeToConstruct.ContainsKey(targetType))
		{
			var instanciationOperationParameterType = ResolveBestInstanciationTypeCandidate(instanciationOperationParameterTypeCandidates, constructorParameterType);
			InstanciationOperationParameterTypesByTypeToConstruct.Add(targetType, instanciationOperationParameterType);
		}
		return _instance;
	}
	static Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = new Dictionary<Type, Type>();
	ConstrainedCollectionTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[targetType];
		var collection = CollectionReadingOperation.Instance.Execute(ref reader, instanciationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
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
		if (instanciationParametersAssignableFromConstructorParameter.Any())
		{
			return instanciationParametersAssignableFromConstructorParameter.First();
		}

		var instanciationParametersAssignableToConstructorParameter = instanciationParameterTypeCandidates
			.Where(instanciationParameter => instanciationParameter.IsAssignableTo(constructorParameter))
			.ToArray();
		if (instanciationParametersAssignableToConstructorParameter.Any())
		{
			return instanciationParametersAssignableToConstructorParameter.First();
		}

		return instanciationParameterTypeCandidates.First();
	}
}

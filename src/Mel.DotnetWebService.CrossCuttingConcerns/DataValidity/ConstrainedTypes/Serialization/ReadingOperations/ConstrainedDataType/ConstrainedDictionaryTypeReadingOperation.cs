using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataType;

class ConstrainedDictionaryTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedDictionaryTypeReadingOperation _instance = new();
	public static ConstrainedDictionaryTypeReadingOperation Instance(Type targetType, Type? constructorParameterType, Type[] instantiationOperationParameterTypeCandidates)
	{
		InstantiationOperationParameterTypesByTypeToConstruct.GetOrAdd(targetType, _ => ResolveBestInstantiationTypeCandidate(instantiationOperationParameterTypeCandidates, constructorParameterType));
		return _instance;
	}
	static readonly ConcurrentDictionary<Type, Type> InstantiationOperationParameterTypesByTypeToConstruct = [];
	ConstrainedDictionaryTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instantiationParameterType = InstantiationOperationParameterTypesByTypeToConstruct[targetType];
		var asDictionary = typeof(Dictionary<,>).MakeGenericType(instantiationParameterType.GetGenericArguments().First().GetGenericArguments());
		var collection = KeyValuePairsReadingOperation.Instance.Execute(ref reader, asDictionary/*instantiationParameterType*/, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
		return firstClassCollection;
	}

	static Type ResolveBestInstantiationTypeCandidate(Type[] instantiationParameterTypeCandidates, Type? constructorParameter)
	{
		if (instantiationParameterTypeCandidates.Contains(constructorParameter))
		{
			return constructorParameter!;
		}

		var instantiationParametersAssignableFromConstructorParameter = instantiationParameterTypeCandidates
			.Where(instantiationParameter => instantiationParameter.IsAssignableFrom(constructorParameter))
			.ToArray();
		if (instantiationParametersAssignableFromConstructorParameter.Length != 0)
		{
			return instantiationParametersAssignableFromConstructorParameter.First();
		}

		var instantiationParametersAssignableToConstructorParameter = instantiationParameterTypeCandidates
			.Where(instantiationParameter => instantiationParameter.IsAssignableTo(constructorParameter))
			.ToArray();
		if (instantiationParametersAssignableToConstructorParameter.Length != 0)
		{
			return instantiationParametersAssignableToConstructorParameter.First();
		}

		return instantiationParameterTypeCandidates.First();
	}
}

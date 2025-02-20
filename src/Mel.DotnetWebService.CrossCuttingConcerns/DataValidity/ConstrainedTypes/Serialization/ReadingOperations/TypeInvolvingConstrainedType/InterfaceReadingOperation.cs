using System.Collections.Concurrent;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class InterfaceReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly InterfaceReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, Type> ConcreteImplementationTypesByInterfaceType = [];
	public static InterfaceReadingOperation Instance(Type interfaceType)
	{
		ConcreteImplementationTypesByInterfaceType.GetOrAdd(
			interfaceType,
			_ => InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(interfaceType));
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var concreteType = ConcreteImplementationTypesByInterfaceType[targetType];
		return ConstrainedTypeConverterReadingOperation
			.For(concreteType)
			.Execute(ref reader, concreteType, options, preComputedOptionsWithoutConstrainedTypeConverter);
	}
}

using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class ClassWithPublicStaticFactoryMethodReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicStaticFactoryMethodReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodReadingOperation Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		StaticFactoryMethodWithMostParametersByType.GetOrAdd(
			type,
			_ => staticFactoryMethods
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First());
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[targetType];
		var parameters = ReadParameterValuesFor(targetType, staticFactoryMethod, ref reader, options, preComputedOptionsWithoutConstrainedTypeConverter);

		var instance = staticFactoryMethod.Invoke(null, parameters);
		return instance;
	}
}

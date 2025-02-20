using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class ClassWithPublicConstructorHavingParametersReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicConstructorHavingParametersReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersReadingOperation Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		ConstructorWithMostParametersByType.GetOrAdd(
			type,
			_ => constructorsWithParameters
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First());
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var constructor = ConstructorWithMostParametersByType[targetType];
		var parameters = ReadParameterValuesFor(targetType, constructor, ref reader, options, preComputedOptionsWithoutConstrainedTypeConverter);

		var instance = constructor.Invoke(parameters);
		return instance;
	}
}

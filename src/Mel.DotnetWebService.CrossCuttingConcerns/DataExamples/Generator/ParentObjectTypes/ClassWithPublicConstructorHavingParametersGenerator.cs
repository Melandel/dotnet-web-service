using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ClassWithPublicConstructorHavingParametersGenerator : ParentObjectGenerator
{
	static readonly ClassWithPublicConstructorHavingParametersGenerator _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersGenerator Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		if (!ConstructorWithMostParametersByType.ContainsKey(type))
		{
			var constructorWithMostParameters = constructorsWithParameters
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			ConstructorWithMostParametersByType.Add(type, constructorWithMostParameters);
		}
		return _instance;
	}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		var parameters = GenerateParameterExamplesFor(constructor, salt);

		var instance = constructor.Invoke(parameters);
		return instance;
	}
}

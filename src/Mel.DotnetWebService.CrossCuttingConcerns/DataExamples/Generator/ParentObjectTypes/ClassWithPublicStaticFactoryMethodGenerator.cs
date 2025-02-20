using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ClassWithPublicStaticFactoryMethodGenerator : ParentObjectGenerator
{
	static readonly ClassWithPublicStaticFactoryMethodGenerator _instance = new();
	static readonly Dictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodGenerator Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		if (!StaticFactoryMethodWithMostParametersByType.ContainsKey(type))
		{
			var staticFactoryMethodWithMostParameters = staticFactoryMethods
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			StaticFactoryMethodWithMostParametersByType.Add(type, staticFactoryMethodWithMostParameters);
		}
		return _instance;
	}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[type];
		var parameters = GenerateParameterExamplesFor(staticFactoryMethod, salt);

		var instance = staticFactoryMethod.Invoke(null, parameters)!;
		return instance;
	}
}


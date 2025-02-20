using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyGenerator : ParentObjectGenerator
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyGenerator _instance = new();
	static readonly Dictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyGenerator Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var singletonAccess = SingletonAccessPropertyByType[type];
		var instance = singletonAccess.GetValue(null)!;
		return instance;
	}
}


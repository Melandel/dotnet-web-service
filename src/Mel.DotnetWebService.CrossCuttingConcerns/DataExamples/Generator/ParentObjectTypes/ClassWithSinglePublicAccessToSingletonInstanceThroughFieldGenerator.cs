using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ClassWithSinglePublicAccessToSingletonInstanceThroughFieldGenerator : ParentObjectGenerator
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughFieldGenerator _instance = new();
	static readonly Dictionary<Type, FieldInfo> SingletonAccessFieldByType = [];
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughFieldGenerator Instance(Type type, FieldInfo singletonAccessField)
	{
		SingletonAccessFieldByType.TryAdd(type, singletonAccessField);
		return _instance;
	}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var singletonAccess = SingletonAccessFieldByType[type];
		var instance = singletonAccess.GetValue(null)!;
		return instance;
	}
}


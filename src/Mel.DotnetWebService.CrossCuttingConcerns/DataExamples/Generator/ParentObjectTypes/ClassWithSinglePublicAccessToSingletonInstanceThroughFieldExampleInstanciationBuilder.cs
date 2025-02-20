using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, FieldInfo> SingletonAccessFieldByType = [];
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder Instance(Type type, FieldInfo singletonAccessField)
	{
		SingletonAccessFieldByType.TryAdd(type, singletonAccessField);
		return _instance;
	}
	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	=> Expression.Field(null, SingletonAccessFieldByType[type]);
}


using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	=> Expression.Property(null, SingletonAccessPropertyByType[type]);
}


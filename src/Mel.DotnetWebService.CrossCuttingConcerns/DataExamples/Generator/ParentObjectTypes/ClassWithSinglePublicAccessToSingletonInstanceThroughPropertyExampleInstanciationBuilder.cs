using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	=> Expression.Property(null, SingletonAccessPropertyByType[type]);
}


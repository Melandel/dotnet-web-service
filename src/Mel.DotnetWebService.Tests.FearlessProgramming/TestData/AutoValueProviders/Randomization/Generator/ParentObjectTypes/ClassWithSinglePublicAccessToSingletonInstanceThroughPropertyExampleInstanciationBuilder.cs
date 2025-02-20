using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	=> Expression.Property(null, SingletonAccessPropertyByType[type]);
}


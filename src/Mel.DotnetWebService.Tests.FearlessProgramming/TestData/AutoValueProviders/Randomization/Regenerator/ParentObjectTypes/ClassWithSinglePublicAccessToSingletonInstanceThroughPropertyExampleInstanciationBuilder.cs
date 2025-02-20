using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	=> Expression.Property(null, SingletonAccessPropertyByType[type]);
}


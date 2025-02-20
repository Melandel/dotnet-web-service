using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder _instance = new();
	static readonly Dictionary<Type, FieldInfo> SingletonAccessFieldByType = [];
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder Instance(Type type, FieldInfo singletonAccessField)
	{
		SingletonAccessFieldByType.TryAdd(type, singletonAccessField);
		return _instance;
	}
	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	=> Expression.Field(null, SingletonAccessFieldByType[type]);
}


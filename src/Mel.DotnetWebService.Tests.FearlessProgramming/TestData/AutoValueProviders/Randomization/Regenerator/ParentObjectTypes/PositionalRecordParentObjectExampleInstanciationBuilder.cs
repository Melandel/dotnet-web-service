using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class PositionalRecordParentObjectExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly PositionalRecordParentObjectExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectExampleInstantiationBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override Expression BuildInstantiationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var constructor = PropertyBasedConstructorsByPositionalRecordType[type];
		return Expression.New(
			constructor,
			GenerateParameterExamplesFor(constructor, generativeSalts));
	}
}

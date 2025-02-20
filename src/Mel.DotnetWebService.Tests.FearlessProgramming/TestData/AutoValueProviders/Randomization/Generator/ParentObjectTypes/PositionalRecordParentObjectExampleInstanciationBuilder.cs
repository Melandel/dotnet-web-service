using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class PositionalRecordParentObjectExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly PositionalRecordParentObjectExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectExampleInstantiationBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var constructor = PropertyBasedConstructorsByPositionalRecordType[type];
		return Expression.New(
			constructor,
			GenerateParameterExamplesFor(constructor, generativeSalts));
	}
}

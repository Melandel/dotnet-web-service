using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class PositionalRecordParentObjectExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly PositionalRecordParentObjectExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectExampleInstanciationBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var constructor = PropertyBasedConstructorsByPositionalRecordType[type];
		return Expression.New(
			constructor,
			GenerateParameterExamplesFor(constructor, generativeSalts));
	}
}

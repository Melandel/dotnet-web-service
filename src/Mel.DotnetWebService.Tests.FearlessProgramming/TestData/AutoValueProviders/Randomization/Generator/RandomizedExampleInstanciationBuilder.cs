using System.Linq.Expressions;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

abstract class RandomizedExampleInstantiationBuilder
{
	public abstract Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts);

	public static Expression BuildFor(Type type, ParameterExpression exampleValue, ParameterExpression generativeSalts)
	{
		return Expression.Block(
			[exampleValue, generativeSalts],
			Expression.Assign(generativeSalts, GenerativeSalts.Create),
			Expression.Assign(exampleValue, BuildFor(type, RandomSalt.Select, generativeSalts)));
	}

	public static Expression BuildFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var typeCategory = type switch
		{
			var t when t.IsEnum => TypeCategory.EnumType,
			var t when t.IsANativeScalarType() => TypeCategory.NativeValueType,
			var t when t.IsASystemTypeThatCanThrowOnInstantiation() => TypeCategory.SystemTypeThatCanThrowOnInstantiation,
			var t when t.ImplementsInterface(typeof(IConstrainedType)) => TypeCategory.ParentObjectType,
			var t when t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => TypeCategory.SystemGenericCollectionType,
			_ => TypeCategory.ParentObjectType
		};

		RandomizedExampleInstantiationBuilder builder = typeCategory switch
		{
			TypeCategory.NativeValueType => NativeValueExampleInstantiationBuilder.Instance,
			TypeCategory.SystemTypeThatCanThrowOnInstantiation => SystemValueTypeThatCanThrowOnInstantiationExampleInstantiationBuilder.Instance,
			TypeCategory.EnumType => EnumExampleInstantiationBuilder.Instance,
			TypeCategory.SystemGenericCollectionType => SystemGenericCollectionExampleInstantiationBuilder.Instance,
			TypeCategory.ParentObjectType => ParentObjectExampleInstantiationBuilder.InstanceSuitedFor(type),
			_ => throw new NotImplementedException()
		};

		return builder.BuildInstantiationExpressionFor(type, currentSalt, generativeSalts);
	}

	enum TypeCategory
	{
		TechnicalDefaultEnumValue = 0,
		NativeValueType = 1,
		SystemTypeThatCanThrowOnInstantiation = 2,
		EnumType = 3,
		SystemGenericCollectionType = 4,
		ParentObjectType = 5
	}
}

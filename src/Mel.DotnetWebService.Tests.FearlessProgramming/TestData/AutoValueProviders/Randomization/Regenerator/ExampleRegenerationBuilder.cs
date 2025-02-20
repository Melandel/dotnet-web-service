using System.Linq.Expressions;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

abstract class ExampleRegenerationBuilder
{
	public abstract Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts);

	public static Expression BuildFor(Type type, ParameterExpression generativeSalts)
	{
		var typeCategory = type switch
		{
			var t when t.IsEnum => TypeCategory.EnumType,
			var t when t.IsANativeScalarType() => TypeCategory.NativeValueType,
			var t when t.IsASystemTypeThatCanThrowOnInstanciation() => TypeCategory.SystemTypeThatCanThrowOnInstanciation,
			var t when t.ImplementsInterface(typeof(IConstrainedType)) => TypeCategory.ParentObjectType,
			var t when t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => TypeCategory.SystemGenericCollectionType,
			_ => TypeCategory.ParentObjectType
		};

		ExampleRegenerationBuilder builder = typeCategory switch
		{
			TypeCategory.NativeValueType => NativeValueExampleInstanciationBuilder.Instance,
			TypeCategory.SystemTypeThatCanThrowOnInstanciation => SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder.Instance,
			TypeCategory.EnumType => EnumExampleInstanciationBuilder.Instance,
			TypeCategory.SystemGenericCollectionType => SystemGenericCollectionExampleInstanciationBuilder.Instance,
			TypeCategory.ParentObjectType => ParentObjectExampleInstanciationBuilder.InstanceSuitedFor(type),
			_ => throw new NotImplementedException()
		};

		return builder.BuildInstanciationExpressionFor(type, generativeSalts);
	}

	enum TypeCategory
	{
		TechnicalDefaultEnumValue = 0,
		NativeValueType = 1,
		SystemTypeThatCanThrowOnInstanciation = 2,
		EnumType = 3,
		SystemGenericCollectionType = 4,
		ParentObjectType = 5
	}
}

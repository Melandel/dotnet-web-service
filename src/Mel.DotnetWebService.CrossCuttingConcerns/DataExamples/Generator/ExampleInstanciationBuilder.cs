using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

abstract class ExampleInstanciationBuilder
{
	public abstract Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt);

	public static Expression BuildFor(Type type, Expression currentSalt)
	{
		var typeCategory = type switch
		{
			var t when t.IsEnum => TypeCategory.EnumType,
			var t when t.IsANativeScalarType() => TypeCategory.NativeValueType,
			var t when t.IsASystemTypeThatCanThrowOnInstanciation() => TypeCategory.SystemTypeThatCanThrowOnInstanciation,
			var t when t.ImplementsInterface(typeof(IConstrainedType)) => TypeCategory.ParentObjectType,
			var t when t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => TypeCategory.SystemGenericCollectionType,
			var t when t.IsADelegateType() => TypeCategory.DelegateType,
			var t when t.IsVoidType() => TypeCategory.VoidType,
			var t when t.IsIOptionsType() => TypeCategory.IOptionsType,
			var t when t.IsInterface => TypeCategory.InterfaceType,

			_ => TypeCategory.ParentObjectType
		};

		ExampleInstanciationBuilder builder = typeCategory switch
		{
			TypeCategory.NativeValueType => NativeValueExampleInstanciationBuilder.Instance,
			TypeCategory.SystemTypeThatCanThrowOnInstanciation => SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder.Instance,
			TypeCategory.EnumType => EnumExampleInstanciationBuilder.Instance,
			TypeCategory.SystemGenericCollectionType => SystemGenericCollectionExampleInstanciationBuilder.Instance,
			TypeCategory.ParentObjectType => ParentObjectExampleInstanciationBuilder.InstanceSuitedFor(type),
			TypeCategory.DelegateType => DelegateExampleInstanciationBuilder.Instance,
			TypeCategory.VoidType => VoidExampleInstanciationBuilder.Instance,
			TypeCategory.IOptionsType=> IOptionsExampleInstanciationBuilder.Instance,
			TypeCategory.InterfaceType => InterfaceExampleInstanciationBuilder.Instance,
			_ => throw new NotImplementedException()
		};

		return builder.BuildInstanciationExpressionFor(type, currentSalt);
	}

	enum TypeCategory
	{
		TechnicalDefaultEnumValue = 0,
		NativeValueType = 1,
		SystemTypeThatCanThrowOnInstanciation = 2,
		EnumType = 3,
		SystemGenericCollectionType = 4,
		ParentObjectType = 5,
		DelegateType = 6,
		VoidType = 7,
		IOptionsType = 8,
		InterfaceType = 9,
	}
}

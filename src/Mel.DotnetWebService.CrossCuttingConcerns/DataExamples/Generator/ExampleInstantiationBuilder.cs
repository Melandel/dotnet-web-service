using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

abstract class ExampleInstantiationBuilder
{
	public abstract Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough);

	public static Expression BuildFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		if (type.ContainsGenericParameters)
		{
			throw new NotSupportedException($"{nameof(ExampleInstantiationBuilder)}.{nameof(BuildFor)}() does not support types featuring unresolved generic parameters ({type.GetName()})");
		}

		var typeCategory = type switch
		{
			var t when t.IsEnum => TypeCategory.EnumType,
			var t when t.IsANativeScalarType() => TypeCategory.NativeValueType,
			var t when t.IsASystemTypeThatCanThrowOnInstantiation() => TypeCategory.SystemTypeThatCanThrowOnInstantiation,
			var t when t.IsATypeWeProvideHardCodedExamplesFor() => TypeCategory.TypeWeProvideHardCodedExamplesFor,
			var t when t.ImplementsInterface(typeof(IConstrainedType)) => TypeCategory.ParentObjectType,
			var t when t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => TypeCategory.SystemGenericCollectionType,
			var t when t.IsADelegateType() => TypeCategory.DelegateType,
			var t when t.IsVoidType() => TypeCategory.VoidType,
			var t when t.IsIOptionsType() => TypeCategory.IOptionsType,
			var t when t.IsInterface => TypeCategory.InterfaceType,

			_ => TypeCategory.ParentObjectType
		};

		ExampleInstantiationBuilder builder = typeCategory switch
		{
			TypeCategory.NativeValueType => NativeValueExampleInstantiationBuilder.Instance,
			TypeCategory.SystemTypeThatCanThrowOnInstantiation => SystemValueTypeThatCanThrowOnInstantiationExampleInstantiationBuilder.Instance,
			TypeCategory.EnumType => EnumExampleInstantiationBuilder.Instance,
			TypeCategory.SystemGenericCollectionType => SystemGenericCollectionExampleInstantiationBuilder.Instance,
			TypeCategory.TypeWeProvideHardCodedExamplesFor => TypeWeProvideHardCodedExamplesForExampleInstantiationBuilder.Instance,
			TypeCategory.ParentObjectType => ParentObjectExampleInstantiationBuilder.InstanceSuitedFor(type),
			TypeCategory.DelegateType => DelegateExampleInstantiationBuilder.Instance,
			TypeCategory.VoidType => VoidExampleInstantiationBuilder.Instance,
			TypeCategory.IOptionsType=> IOptionsExampleInstantiationBuilder.Instance,
			TypeCategory.InterfaceType => InterfaceExampleInstantiationBuilder.Instance,
			_ => throw new NotImplementedException()
		};

		return builder.BuildInstantiationExpressionFor(type, currentSalt, parentObjectsAlreadyRecursedThrough);
	}

	enum TypeCategory
	{
		TechnicalDefaultEnumValue = 0,
		NativeValueType = 1,
		SystemTypeThatCanThrowOnInstantiation = 2,
		TypeWeProvideHardCodedExamplesFor = 3,
		EnumType = 4,
		SystemGenericCollectionType = 5,
		ParentObjectType = 6,
		DelegateType = 7,
		VoidType = 8,
		IOptionsType = 9,
		InterfaceType = 10,
	}
}

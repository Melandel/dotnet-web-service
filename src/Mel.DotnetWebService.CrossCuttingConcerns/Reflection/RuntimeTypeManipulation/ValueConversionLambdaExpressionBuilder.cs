using System.Collections;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

static class ValueConversionLambdaExpressionBuilder
{
	public static LambdaExpression Build(Type sourceCollectionItemType, Type destinationCollectionItemType)
	{
		var parameterExpression = Expression.Parameter(sourceCollectionItemType, "source");
		var reusableExpression = BuildReusableExpression(parameterExpression, sourceCollectionItemType, destinationCollectionItemType);

		return reusableExpression is LambdaExpression asLambdaExpression
			? asLambdaExpression
			: Expression.Lambda(reusableExpression, parameterExpression);
	}
	static Expression BuildReusableExpression(ParameterExpression parameterExpression, Type sourceCollectionItemType, Type destinationCollectionItemType)
	=> (sourceCollectionItemType, destinationCollectionItemType) switch
	{
		(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IDictionary)) || itm2.IsOrImplementsInterface(typeof(IDictionary)) => CollectionConversionCallExpressionBuilder.Build(parameterExpression, itm2),
		(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IEnumerable)) || itm2.IsOrImplementsInterface(typeof(IEnumerable)) => CollectionConversionCallExpressionBuilder.Build(parameterExpression, itm2),
		(var itm1, var itm2) when itm1.IsKeyValuePairType(out _, out _) && itm2.IsKeyValuePairType(out _, out _) => FromKeyValuePair1ToKeyValuePair2(parameterExpression, itm2),
		(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && itm2 == constrainedTypeInfo1.RootType => constrainedTypeInfo1.ConvertorToNativeRootTypeExpression,
		(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) && !itm1.IsOrInvolvesAConstrainedType() => Expression.Call(constrainedTypeInfo2.InstanciationMethod, parameterExpression),
		(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) => FromConstrainedTypeToLessConstrainedType(parameterExpression, constrainedTypeInfo1, constrainedTypeInfo2),
		(var itm1, var itm2) when !itm1.IsOrInvolvesAConstrainedType() && !itm2.IsOrInvolvesAConstrainedType() => (itm1, itm2) switch
		{
			_ when itm1.IsEnum && itm2 == typeof(string) => FromEnumToString(parameterExpression),
			_ when itm1.IsEnum && itm2 == typeof(   int) => FromEnumToInt(parameterExpression),
			_ when itm1 == typeof(string) && itm2.IsEnum => FromStringToEnum(parameterExpression, itm2),
			_ when itm1 == typeof(   int) && itm2.IsEnum => FromIntToEnum(parameterExpression, itm2),
			_ when itm1 == typeof(int) && itm2.IsEnum => FromIntToEnum(parameterExpression, itm2),
			_=> ToDestinationType(parameterExpression, itm2),
		},
		(_, var itm2) => ToDestinationType(parameterExpression, itm2),
	};

	static MethodCallExpression FromConstrainedTypeToLessConstrainedType(ParameterExpression parameterExpression, ConstrainedTypeInfo sourceConstrainedTypeInfo, ConstrainedTypeInfo destinationConstrainedTypeInfo)
	=> Expression.Call(
		destinationConstrainedTypeInfo.InvokableInstanciationMethod,
		Expression.Invoke(
			sourceConstrainedTypeInfo.ConvertorToNativeRootTypeExpression,
			parameterExpression));

	static LambdaExpression FromKeyValuePair1ToKeyValuePair2(ParameterExpression parameterExpression, Type destinationType)
	{
		if (!parameterExpression.Type.IsKeyValuePairType(out var keyType1, out var valueType1) || !destinationType.IsKeyValuePairType(out var keyType2, out var valueType2))
		{
			throw new InvalidOperationException();
		}
		var body = Expression.New(
				destinationType.GetConstructor([ keyType2, valueType2 ])!,
				Expression.Invoke(
					ValueConversionLambdaExpressionBuilder.Build(keyType1, keyType2),
					Expression.Property(parameterExpression, "Key")),
				Expression.Invoke(
					ValueConversionLambdaExpressionBuilder.Build(valueType1, valueType2),
					Expression.Property(parameterExpression, "Value")));

		return Expression.Lambda(body, parameterExpression);
	}

	static MethodCallExpression FromEnumToString(ParameterExpression parameterExpression)
	=> Expression.Call(parameterExpression, nameof(object.ToString), Type.EmptyTypes);

	static UnaryExpression FromStringToEnum(ParameterExpression parameterExpression, Type enumType)
	=> Expression.Convert(
		Expression.Call(typeof(Enum), nameof(Enum.Parse), Type.EmptyTypes, Expression.Constant(enumType), parameterExpression, Expression.Constant(true)),
		enumType);

	public static UnaryExpression FromEnumToInt(ParameterExpression parameterExpression)
	=> Expression.Convert(parameterExpression, typeof(int));

	public static UnaryExpression FromIntToEnum(ParameterExpression parameterExpression, Type enumType)
	=> Expression.Convert(parameterExpression, enumType);

	public static UnaryExpression ToDestinationType(ParameterExpression parameterExpression, Type destinationType)
	=> Expression.Convert(parameterExpression, destinationType);
}

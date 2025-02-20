using System.Collections;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

static class SelectClauseLambdaExpressionBuilder
{
	public static LambdaExpression Build(Type sourceCollectionItemType, Type destinationCollectionItemType)
	{
		var collectionItemParameterExpression = Expression.Parameter(sourceCollectionItemType, "x");
		var conversionExpression = ObjectConversionBuilder.BuildConversionExpressionFor(sourceCollectionItemType, destinationCollectionItemType, collectionItemParameterExpression);

		return Expression.Lambda(conversionExpression, collectionItemParameterExpression);
	}
}

class ObjectConversionBuilder
{
	public static Expression BuildConversionExpressionFor(Type sourceType, Type destinationType, Expression currentExpression)
	{
		if (sourceType == destinationType)
		{
			return currentExpression;
		}

		Expression sourceToDestinationItemTypeExpression = (sourceType, destinationType) switch
		{
			(var itm1, var itm2) when itm1.IsKeyValuePairType(out _, out _) && itm2.IsKeyValuePairType(out _, out _) => FromKeyValuePair1ToKeyValuePair2(currentExpression, itm2),
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && itm2 == constrainedTypeInfo1.RootType => constrainedTypeInfo1.BuildConvertorToNativeRootTypeMethodCallExpression(currentExpression),
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) && !itm1.IsOrInvolvesAConstrainedType() => Expression.Call(constrainedTypeInfo2.InstanciationMethod, currentExpression),
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) => FromConstrainedTypeToLessConstrainedType(currentExpression, constrainedTypeInfo1, constrainedTypeInfo2),
			(var itm1, var itm2) when !itm1.IsOrInvolvesAConstrainedType() && !itm2.IsOrInvolvesAConstrainedType() => (itm1, itm2) switch
			{
				_ when itm1.IsEnum && itm2 == typeof(string) => FromEnumToString(currentExpression),
				_ when itm1.IsEnum && itm2 == typeof(int) => FromEnumToInt(currentExpression),
				_ when itm1 == typeof(string) && itm2.IsEnum => FromStringToEnum(currentExpression, itm2),
				_ when itm1 == typeof(int) && itm2.IsEnum => FromIntToEnum(currentExpression, itm2),
				_ => ToDestinationType(currentExpression, itm2),
			},
			(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IDictionary)) || itm2.IsOrImplementsInterface(typeof(IDictionary)) => CollectionConversionCallExpressionBuilder.Build(currentExpression, itm2),
			(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IEnumerable)) || itm2.IsOrImplementsInterface(typeof(IEnumerable)) => CollectionConversionCallExpressionBuilder.Build(currentExpression, itm2),
			(_, var itm2) => ToDestinationType(currentExpression, itm2),
		};

		if (currentExpression.Type == sourceType)
		{
			return sourceToDestinationItemTypeExpression;
		}

		var parameterExpressionLooseCollectionTypeToSourceCollectionTypeSelectClause = BuildConversionExpressionFor(currentExpression.Type, sourceType, currentExpression);
		Expression conversionToSourceCollectionItemType = parameterExpressionLooseCollectionTypeToSourceCollectionTypeSelectClause switch
		{
			LambdaExpression conversionLambda => Expression.Invoke(conversionLambda, currentExpression),
			MethodCallExpression conversionMethodCall => Expression.Call(conversionMethodCall.Method, currentExpression),
			UnaryExpression implicitConversion => implicitConversion,
			_ => throw new NotImplementedException()
		};

		if (sourceToDestinationItemTypeExpression is LambdaExpression selectAsLambda)
		{
			return Expression.Invoke(selectAsLambda, conversionToSourceCollectionItemType);
		}
		else if (sourceToDestinationItemTypeExpression is MethodCallExpression selectAsMethodCall)
		{
			return Expression.Call(
				selectAsMethodCall.Method,
				conversionToSourceCollectionItemType);
		}
		else if (sourceToDestinationItemTypeExpression is UnaryExpression selectAsImplicitConversion)
		{
			return Expression.Convert(currentExpression, sourceType);
		}
		else
		{
			throw new NotImplementedException();
		}
	}

	static MethodCallExpression FromConstrainedTypeToLessConstrainedType(Expression currentExpression, ConstrainedTypeInfo sourceConstrainedTypeInfo, ConstrainedTypeInfo destinationConstrainedTypeInfo)
	=> Expression.Call(
		destinationConstrainedTypeInfo.InvokableInstanciationMethod,
		Expression.Invoke(
			sourceConstrainedTypeInfo.ConvertorToNativeRootTypeExpression,
			currentExpression));

	static NewExpression FromKeyValuePair1ToKeyValuePair2(Expression currentExpression, Type destinationType)
	{
		if (!currentExpression.Type.IsKeyValuePairType(out var keyType1, out var valueType1) || !destinationType.IsKeyValuePairType(out var keyType2, out var valueType2))
		{
			throw new InvalidOperationException();
		}
		return Expression.New(
			destinationType.GetConstructor([keyType2, valueType2])!,
			Expression.Invoke(
				SelectClauseLambdaExpressionBuilder.Build(keyType1, keyType2),
				Expression.Property(currentExpression, "Key")),
			Expression.Invoke(
				SelectClauseLambdaExpressionBuilder.Build(valueType1, valueType2),
				Expression.Property(currentExpression, "Value")));
	}

	static MethodCallExpression FromEnumToString(Expression currentExpression)
	=> Expression.Call(currentExpression, nameof(object.ToString), Type.EmptyTypes);

	static UnaryExpression FromStringToEnum(Expression currentExpression, Type enumType)
	=> Expression.Convert(
		Expression.Call(typeof(Enum), nameof(Enum.Parse), Type.EmptyTypes, Expression.Constant(enumType), currentExpression, Expression.Constant(true)),
		enumType);

	public static UnaryExpression FromEnumToInt(Expression currentExpression)
	=> Expression.Convert(currentExpression, typeof(int));

	public static UnaryExpression FromIntToEnum(Expression currentExpression, Type enumType)
	=> Expression.Convert(currentExpression, enumType);

	public static UnaryExpression ToDestinationType(Expression currentExpression, Type destinationType)
	=> Expression.Convert(currentExpression, destinationType);
}

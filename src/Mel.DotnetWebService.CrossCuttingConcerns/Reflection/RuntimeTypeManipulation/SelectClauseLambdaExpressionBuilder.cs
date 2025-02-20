using System.Collections;
using System.Linq.Expressions;
using System.Security.AccessControl;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

static class SelectClauseLambdaExpressionBuilder
{
	public static LambdaExpression Build(Type sourceCollectionItemType, Type destinationCollectionItemType)
	{
		var collectionItemParameterExpression = Expression.Parameter(sourceCollectionItemType, "x");
		var reusableExpression = BuildReusableExpression(collectionItemParameterExpression, sourceCollectionItemType, destinationCollectionItemType);

		return Expression.Lambda(reusableExpression, collectionItemParameterExpression);
	}
	static Expression BuildReusableExpression(ParameterExpression collectionItemParameterExpression, Type sourceCollectionItemType, Type destinationCollectionItemType)
	{
		Expression sourceToDestinationItemTypeExpression = (sourceCollectionItemType, destinationCollectionItemType) switch
		{
			(var itm1, var itm2) when itm1.IsKeyValuePairType(out _, out _) && itm2.IsKeyValuePairType(out _, out _) => FromKeyValuePair1ToKeyValuePair2(collectionItemParameterExpression, itm2), // new
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && itm2 == constrainedTypeInfo1.RootType => constrainedTypeInfo1.BuildConvertorToNativeRootTypeMethodCallExpression(collectionItemParameterExpression), // call
			//(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && itm2 != constrainedTypeInfo1.RootType => BuildReusableExpression(collectionItemParameterExpression, constrainedTypeInfo1.RootType, itm2),
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) && !itm1.IsOrInvolvesAConstrainedType() => Expression.Call(constrainedTypeInfo2.InstanciationMethod, collectionItemParameterExpression), // Call
			(var itm1, var itm2) when ConstrainedTypeInfos.TryGet(itm1, out var constrainedTypeInfo1) && ConstrainedTypeInfos.TryGet(itm2, out var constrainedTypeInfo2) => FromConstrainedTypeToLessConstrainedType(collectionItemParameterExpression, constrainedTypeInfo1, constrainedTypeInfo2), // Call
			(var itm1, var itm2) when !itm1.IsOrInvolvesAConstrainedType() && !itm2.IsOrInvolvesAConstrainedType() => (itm1, itm2) switch
			{
				_ when itm1.IsEnum && itm2 == typeof(string) => FromEnumToString(collectionItemParameterExpression), // call
				_ when itm1.IsEnum && itm2 == typeof(int) => FromEnumToInt(collectionItemParameterExpression), // unary
				_ when itm1 == typeof(string) && itm2.IsEnum => FromStringToEnum(collectionItemParameterExpression, itm2), // unary
				_ when itm1 == typeof(int) && itm2.IsEnum => FromIntToEnum(collectionItemParameterExpression, itm2), // unary
				_ => ToDestinationType(collectionItemParameterExpression, itm2), // unary
			},
			(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IDictionary)) || itm2.IsOrImplementsInterface(typeof(IDictionary)) => CollectionConversionCallExpressionBuilder.Build(collectionItemParameterExpression, itm2), // call
			(var itm1, var itm2) when itm1.IsOrImplementsInterface(typeof(IEnumerable)) || itm2.IsOrImplementsInterface(typeof(IEnumerable)) => CollectionConversionCallExpressionBuilder.Build(collectionItemParameterExpression, itm2), // call
			(_, var itm2) => ToDestinationType(collectionItemParameterExpression, itm2), // unary
		};

		if (collectionItemParameterExpression.Type == sourceCollectionItemType)
		{
			return sourceToDestinationItemTypeExpression;
		}

		var parameterExpressionLooseCollectionTypeToSourceCollectionTypeSelectClause = BuildReusableExpression(collectionItemParameterExpression, collectionItemParameterExpression.Type, sourceCollectionItemType);
		Expression conversionToSourceCollectionItemType = parameterExpressionLooseCollectionTypeToSourceCollectionTypeSelectClause switch
		{
			LambdaExpression conversionLambda => Expression.Invoke(conversionLambda, collectionItemParameterExpression),
			MethodCallExpression conversionMethodCall => Expression.Call(conversionMethodCall.Method, collectionItemParameterExpression),
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
			return Expression.Convert(collectionItemParameterExpression, sourceCollectionItemType);
		}
		else
		{
			throw new NotImplementedException();
		}
	}

	static MethodCallExpression FromConstrainedTypeToLessConstrainedType(ParameterExpression parameterExpression, ConstrainedTypeInfo sourceConstrainedTypeInfo, ConstrainedTypeInfo destinationConstrainedTypeInfo)
	=> Expression.Call(
		destinationConstrainedTypeInfo.InvokableInstanciationMethod,
		Expression.Invoke(
			sourceConstrainedTypeInfo.ConvertorToNativeRootTypeExpression,
			parameterExpression));

	static NewExpression FromKeyValuePair1ToKeyValuePair2(ParameterExpression parameterExpression, Type destinationType)
	{
		if (!parameterExpression.Type.IsKeyValuePairType(out var keyType1, out var valueType1) || !destinationType.IsKeyValuePairType(out var keyType2, out var valueType2))
		{
			throw new InvalidOperationException();
		}
		return Expression.New(
			destinationType.GetConstructor([ keyType2, valueType2 ])!,
			Expression.Invoke(
				SelectClauseLambdaExpressionBuilder.Build(keyType1, keyType2),
				Expression.Property(parameterExpression, "Key")),
			Expression.Invoke(
				SelectClauseLambdaExpressionBuilder.Build(valueType1, valueType2),
				Expression.Property(parameterExpression, "Value")));
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

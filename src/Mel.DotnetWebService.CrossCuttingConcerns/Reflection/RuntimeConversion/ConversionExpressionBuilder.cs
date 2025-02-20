using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeConversion;

public abstract class ConversionExpressionBuilder
{
	static readonly Dictionary<Type, Dictionary<Type, Expression>> CachedConversionExpressionsByDestinationTypeBySourceType = [];
	protected abstract Expression BuildConversionExpressionFor(Type sourceType, Type destinationType);

	public static Expression BuildFor(Type sourceType, Type destinationType)
	{
		if (IsConversionExpressionAlreadyCached(sourceType, destinationType, out var conversionExpression))
		{
			return conversionExpression;
		}

		conversionExpression = BuildConversionExpression(sourceType, destinationType);
		return conversionExpression;
	}

	static bool IsConversionExpressionAlreadyCached(Type sourceType, Type destinationType, out Expression conversionExpression)
	{
		if (CachedConversionExpressionsByDestinationTypeBySourceType.TryGetValue(sourceType, out var instanciationExpressionsByDestinationType)
			&& instanciationExpressionsByDestinationType.TryGetValue(destinationType, out conversionExpression!))
		{
			return true;
		};
		conversionExpression = null!;
		return false;
	}

	protected static Expression BuildConversionExpression(Type sourceType, Type destinationType)
	{
		if (IsConversionExpressionAlreadyCached(sourceType, destinationType, out var instanciationExpression))
		{
			return instanciationExpression;
		}

		instanciationExpression = Build(sourceType, destinationType);
		Cache(instanciationExpression, sourceType, destinationType);
		return instanciationExpression;
	}

	static Expression Build(Type sourceType, Type destinationType)
	{
		var conversionCategory = ConversionCategoryResolver.Resolve(sourceType, destinationType);
		var builder = ConversionExpressionBuilderResolver.Resolve(conversionCategory, sourceType, destinationType);
		var conversionExpression = builder.BuildConversionExpressionFor(sourceType, destinationType);

		return conversionExpression;
	}

	static void Cache(Expression conversionExpression, Type sourceType, Type destinationType)
	{
		if (CachedConversionExpressionsByDestinationTypeBySourceType.TryGetValue(sourceType, out var conversionExpressionsByDestinationType))
		{
			conversionExpressionsByDestinationType.Add(destinationType, conversionExpression);
		}
		else
		{
			conversionExpressionsByDestinationType = new Dictionary<Type, Expression>() { { destinationType, conversionExpression } };
			CachedConversionExpressionsByDestinationTypeBySourceType.Add(sourceType, conversionExpressionsByDestinationType);
		}
	}
}


enum ConversionCategory
{
	TechnicalDefaultEnumValue = 0,
}
class ConversionCategoryResolver
{
	public static ConversionCategory Resolve(Type sourceType, Type destinationType)
	{
		return ConversionCategory.TechnicalDefaultEnumValue;
	}
}
class ConversionExpressionBuilderResolver
{
	public static ConversionExpressionBuilder Resolve(ConversionCategory conversionCategory, Type sourceType, Type destinationType)
	{
		throw new NotImplementedException();
	}
}

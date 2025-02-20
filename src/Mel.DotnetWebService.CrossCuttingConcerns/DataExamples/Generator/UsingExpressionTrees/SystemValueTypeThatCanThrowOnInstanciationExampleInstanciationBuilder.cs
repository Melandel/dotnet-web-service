using System.Globalization;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder Instance = new();
	static readonly Dictionary<Type, string> ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation = new()
	{
		{ typeof(CultureInfo), nameof(ExampleValues.ForCultureInfoType)},
		{ typeof(RegionInfo),  nameof(ExampleValues.ForRegionInfoType) },
		{ typeof(Version),     nameof(ExampleValues.ForVersionType)    },
		{ typeof(Uri),         nameof(ExampleValues.ForUriType)        },
	};

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var values = Expression.Field(null, typeof(ExampleValues), ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(valuesToArrayConvertor, values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(currentSalt, length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

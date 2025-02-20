using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder Instance = new();
	static readonly Dictionary<Type, FieldInfo> ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation = new()
	{
		{ typeof(CultureInfo), typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForCultureInfoType))! },
		{ typeof(RegionInfo),  typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForRegionInfoType))! },
		{ typeof(Version),     typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForVersionType))!    },
		{ typeof(Uri),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForUriType))!        },
	};

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var values = Expression.Field(null, ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(valuesToArrayConvertor, values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(currentSalt, length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

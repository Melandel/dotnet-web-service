using System.Collections.Frozen;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class SystemValueTypeThatCanThrowOnInstantiationExampleInstantiationBuilder : ExampleRegenerationBuilder
{
	public static readonly SystemValueTypeThatCanThrowOnInstantiationExampleInstantiationBuilder Instance = new();
	static readonly FrozenDictionary<Type, FieldInfo> ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstantiation = new Dictionary<Type, FieldInfo>()
	{
		{ typeof(CultureInfo), typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForCultureInfoType)) },
		{ typeof(RegionInfo),  typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForRegionInfoType)) },
		{ typeof(Version),     typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForVersionType))    },
		{ typeof(Uri),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForUriType))        },
	}.ToFrozenDictionary();

	public override Expression BuildInstantiationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var values = Expression.Field(null, ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstantiation[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(valuesToArrayConvertor, values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(GenerativeSalts.Dequeue(generativeSalts), length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

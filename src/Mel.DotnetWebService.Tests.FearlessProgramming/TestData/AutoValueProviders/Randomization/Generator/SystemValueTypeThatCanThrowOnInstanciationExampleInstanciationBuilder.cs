using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder : RandomizedExampleInstanciationBuilder
{
	public static readonly SystemValueTypeThatCanThrowOnInstanciationExampleInstanciationBuilder Instance = new();
	static readonly Dictionary<Type, FieldInfo> ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation = new()
	{
		{ typeof(CultureInfo), typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForCultureInfoType)) },
		{ typeof(RegionInfo),  typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForRegionInfoType)) },
		{ typeof(Version),     typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForVersionType))    },
		{ typeof(Uri),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForUriType))        },
	};

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var examples = Expression.Field(null, ExampleValuesFieldNameBySystemValueTypeThatCanThrowOnInstanciation[type]);
		var conversionToArrayType = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var examplesAsArray = Expression.Variable(type.MakeArrayType(), "examplesAsArray");
		var index = Expression.Variable(typeof(int), "index");
		return Expression.Block(
			[examplesAsArray, index],
			Expression.Assign(examplesAsArray, Expression.Invoke(conversionToArrayType, examples)),
			Expression.Assign(index,           Expression.Modulo(currentSalt, Expression.ArrayLength(examplesAsArray))),

			GenerativeSalts.Enqueue(generativeSalts, index),
			Expression.ArrayIndex(examplesAsArray, index));
	}
}

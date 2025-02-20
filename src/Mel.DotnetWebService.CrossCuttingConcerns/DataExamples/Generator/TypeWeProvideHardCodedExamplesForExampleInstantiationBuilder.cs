using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class TypeWeProvideHardCodedExamplesForExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	public static readonly TypeWeProvideHardCodedExamplesForExampleInstantiationBuilder Instance = new();
	static readonly FrozenDictionary<Type, FieldInfo> ValidExampleValuesFieldByType = new Dictionary<Type, FieldInfo>()
	{
		{ typeof(Type), typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForTypeType))!           },
	}.ToFrozenDictionary();

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var values = Expression.Field(null, ValidExampleValuesFieldByType[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(
			ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression,
			values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(currentSalt, length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class NativeValueExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly NativeValueExampleInstanciationBuilder Instance = new();
	static readonly Dictionary<Type, string> ExampleValuesFieldNameByNativeType = new()
	{
		{ typeof(bool),           nameof(ExampleValues.ForBoolType)           },
		{ typeof(byte),           nameof(ExampleValues.ForByteType)           },
		{ typeof(DateTime),       nameof(ExampleValues.ForDateTimeType)       },
		{ typeof(DateTimeOffset), nameof(ExampleValues.ForDateTimeOffsetType) },
		{ typeof(decimal),        nameof(ExampleValues.ForDecimalType)        },
		{ typeof(double),         nameof(ExampleValues.ForDoubleType)         },
		{ typeof(float),          nameof(ExampleValues.ForFloatType)          },
		{ typeof(Guid),           nameof(ExampleValues.ForGuidType)           },
		{ typeof(int),            nameof(ExampleValues.ForIntType)            },
		{ typeof(long),           nameof(ExampleValues.ForLongType)           },
		{ typeof(nint),           nameof(ExampleValues.ForNIntType)           },
		{ typeof(nuint),          nameof(ExampleValues.ForNUIntType)          },
		{ typeof(sbyte),          nameof(ExampleValues.ForSByteType)          },
		{ typeof(short),          nameof(ExampleValues.ForShortType)          },
		{ typeof(string),         nameof(ExampleValues.ForStringType)         },
		{ typeof(uint),           nameof(ExampleValues.ForUIntType)           },
		{ typeof(ulong),          nameof(ExampleValues.ForULongType)          },
		{ typeof(ushort),         nameof(ExampleValues.ForUShortType)         },
	};

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var values = Expression.Field(null, typeof(ExampleValues), ExampleValuesFieldNameByNativeType[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(valuesToArrayConvertor, values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(currentSalt, length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

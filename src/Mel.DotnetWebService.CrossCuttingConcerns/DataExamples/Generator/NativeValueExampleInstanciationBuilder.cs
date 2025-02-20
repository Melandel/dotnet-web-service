using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class NativeValueExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly NativeValueExampleInstanciationBuilder Instance = new();
	static readonly Dictionary<Type, FieldInfo> ValidExampleValuesFieldByNativeType = new()
	{
		{ typeof(bool),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForBoolType))!           },
		{ typeof(byte),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForByteType))!           },
		{ typeof(DateTime),       typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForDateTimeType))!       },
		{ typeof(DateTimeOffset), typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForDateTimeOffsetType))! },
		{ typeof(decimal),        typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForDecimalType))!        },
		{ typeof(double),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForDoubleType))!         },
		{ typeof(float),          typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForFloatType))!          },
		{ typeof(Guid),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForGuidType))!           },
		{ typeof(int),            typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForIntType))!            },
		{ typeof(long),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForLongType))!           },
		{ typeof(nint),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForNIntType))!           },
		{ typeof(nuint),          typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForNUIntType))!          },
		{ typeof(sbyte),          typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForSByteType))!          },
		{ typeof(short),          typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForShortType))!          },
		{ typeof(string),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForStringType))!         },
		{ typeof(uint),           typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForUIntType))!           },
		{ typeof(ulong),          typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForULongType))!          },
		{ typeof(ushort),         typeof(ValidExampleValues).GetField(nameof(ValidExampleValues.ForUShortType))!         },
	};

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var values = Expression.Field(null, ValidExampleValuesFieldByNativeType[type]);
		var valuesToArrayConvertor = ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression;

		var valuesAsArray = Expression.Invoke(
			ConstrainedTypeInfos.Get(typeof(ArrayOfUniqueValuesWithAtLeast2Items<>).MakeGenericType(type)).ConvertorToNativeRootTypeExpression,
			values);

		var length = Expression.ArrayLength(valuesAsArray);
		var index = Expression.Modulo(currentSalt, length);

		return Expression.ArrayIndex(valuesAsArray, index);
	}
}

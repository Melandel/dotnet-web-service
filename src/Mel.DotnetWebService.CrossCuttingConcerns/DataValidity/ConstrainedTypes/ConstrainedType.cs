namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedType
{
	public static readonly Dictionary<Type, Type> DefaultValueForbiddingTypesByNativeScalarType = new Dictionary<Type, Type>
	{
		{ typeof(string), typeof(NonEmptyString) },
		{ typeof(Guid), typeof(NonEmptyGuid) },
		{ typeof(DateTime), typeof(NonDefaultDateTime) },
		{ typeof(DateTimeOffset), typeof(NonDefaultDateTimeOffset) },
		{ typeof(decimal), typeof(NonZeroDecimal) },
		{ typeof(double), typeof(NonZeroDouble) },
		{ typeof(float), typeof(NonZeroFloat) },
		{ typeof(byte), typeof(NonZeroByte) },
		{ typeof(int), typeof(NonZeroInt) },
		{ typeof(long), typeof(NonZeroLong) },
		{ typeof(sbyte), typeof(NonZeroSByte) },
		{ typeof(short), typeof(NonZeroShort) },
		{ typeof(uint), typeof(NonZeroUInt) },
		{ typeof(ulong), typeof(NonZeroULong) },
		{ typeof(ushort), typeof(NonZeroUShort) },
	};
}
